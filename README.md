# Coffee Shop Backend - .NET 8 Web API

This project contains a .NET 8 Web API developed for a coffee shop. The API supports various functionalities such as inventory management, product operations, user authentication, JWT-based validation, order creation, and user roles.

## Getting Started

To run the project on your local machine, follow these steps:

1. Clone the repository:
    ```bash
    git clone https://github.com/tw4/Coffee-Shop-Backend.git
    ```
2. Navigate to the project directory:
    ```bash
    cd coffee-shop-backend
    ```
3. Install the required dependencies:
    ```bash
    dotnet restore
    ```
4. Apply database migrations:
    ```bash
    dotnet ef database update
    ```
5. Run the project:
    ```bash
    dotnet run
    ```
6. The API will run by default at `https://localhost:5001`.

## Usage

You can perform the following functionalities using the API:

- Inventory Management: Listing, adding, updating, and deleting products.
- User Management: Creating, updating, and deleting users.
- JWT User Authentication: User login and token retrieval.
- User Roles: Managing user roles and checking assigned roles.
- Order Creation: Creating and managing orders based on products.

## Technologies

This project utilizes the following technologies:

- .NET 8
- Entity Framework Core
- JWT Authentication
- Swagger/OpenAPI documentation
- Redis
- Elasticsearch
- Docker
- Azure SQL Edge
- Stripe
- Serilog

## Contributing

If you'd like to contribute to this project, please check the `CONTRIBUTING.md` file and feel free to submit a pull request.

## License

This project is licensed under the [MIT License](LICENSE).
