using coffee_shop_backend.Business.Abstracts;
using coffee_shop_backend.Contexs;
using coffee_shop_backend.Dto.Paymant;
using coffee_shop_backend.Entitys.Concreates;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using Order = coffee_shop_backend.Entitys.Concreates.Order;
using Product = coffee_shop_backend.Entitys.Concreates.Product;

namespace coffee_shop_backend.Business.Concreates;

public class PaymentManager: IPaymentServices
{
    private readonly IConfiguration _configuration;
    private readonly CoffeeShopDbContex _coffeeShopDbContex;
    private readonly IJwtServices _jwtServices;

    public PaymentManager(IConfiguration configuration, CoffeeShopDbContex coffeeShopDbContex, IJwtServices jwtServices)
    {
        _configuration = configuration;
        _coffeeShopDbContex = coffeeShopDbContex;
        _jwtServices = jwtServices;
    }
    public Session Create(CreatePaymenRequest request ,string token)
    {
        Product product = _coffeeShopDbContex.Products.FirstOrDefault(p => p.Id == request.productId);

        if (product == null)
        {
            return null;
        }

        // Get the domain name from appsettings.json
        var domain = _configuration["Domain"];
        // Create the session
        var options = new Stripe.Checkout.SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                //  SessionLineItemOptions is used to create the line item
                new SessionLineItemOptions
                {
                    // PriceData is used to create the price data
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = product.Price,
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = product.Name,
                            Description = product.Description,
                            Images = new List<string>
                            {
                                product.ImageUrl
                            },
                        },
                    },
                    Quantity = 1,
                },
            },
            Mode = "payment",
            SuccessUrl = domain + "/success/",
            CancelUrl = domain + "/cancel/",
        };

        var service = new SessionService();
        // Add the token to the session metadata
        options.Metadata = new Dictionary<string, string>
        {
            {"token", token}
        };
        // Create the session
        Session session = service.Create(options);
        return session;
    }

    public IActionResult PaymentIntentSucceeded(PaymentIntent paymentIntent)
    {

        // get payment method
        var paymentMethodService = new PaymentMethodService();
        var paymentMethod = paymentMethodService.Get(paymentIntent.PaymentMethodId);

        // Get the session list options to retrieve the line items
        var options = new SessionListOptions()
        {
            PaymentIntent = paymentIntent?.Id,
            Expand = new List<string> { "data.line_items" },
        };

        // Get the session
        var service = new SessionService();
        var session = service.List(options);
        // Get the line item
        var lineItem = session.Data[0].LineItems.Data[0];
        // Get the product
        Product product = _coffeeShopDbContex.Products.FirstOrDefault(p => p.Name.Equals(lineItem.Description));
        // Check if product exists
        if (product == null)
        {
            return new NotFoundObjectResult(new { message = "Product not found", success = false });
        }
        // Get the user id from token metadata
        long userId = _jwtServices.GetUserIdFromToken(session.Data[0].Metadata["token"]);

        // create order
        Order newOrder = new Order()
        {
            Status = EnumOrderStatus.Waiting,
            ProductId = product.Id,
            UserId = userId,
            PaymentDate = DateTime.Now,
            FullName = paymentMethod.BillingDetails.Name,
            Address = "",
            Email = paymentMethod.BillingDetails.Email,
        };
        // add order to db
        _coffeeShopDbContex.Orders.Add(newOrder);
        // save changes
        try
        {
            _coffeeShopDbContex.SaveChanges();
            return new OkResult();
        }
        catch (Exception e)
        {
            return new BadRequestObjectResult(new {message = "Error while saving order", success = false});
        }
    }
}