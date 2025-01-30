using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using InventoryManagement.Api.Helpers;

namespace InventoryManagement.Api.Filters;

public class ValidationFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if(!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

            context.Result = ApiResponseHelper.BadRequest("❌ Error de validación", errors);
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // No se necesita lógica aquí
    }
}
