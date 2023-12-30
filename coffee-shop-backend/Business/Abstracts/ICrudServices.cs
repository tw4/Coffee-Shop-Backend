using Microsoft.AspNetCore.Mvc;

namespace coffee_shop_backend.Business.Abstracts;

public interface ICrudServices<TAddRequest, TUpdateRequest>
{
    public IActionResult Add(TAddRequest request, string token);
    public IActionResult Update(TUpdateRequest request, string token);
    public IActionResult DeleteById(long id, string token);
    public IActionResult GetById(long id, string token);
}