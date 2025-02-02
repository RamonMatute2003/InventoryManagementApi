using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Shared.Enums;
using InventoryManagement.Shared.Models;

namespace InventoryManagement.Api.Helpers;

public static class ApiResponseHelper
{
    public static IActionResult GenerateResponse<T>(HttpStatusCode statusCode, string message, T body = default!, object error = null!)
    {
        return new ObjectResult(new ApiResponse<T>
        {
            Code = (int) statusCode,
            Message = message,
            Body = body,
            Error = error
        })
        {
            StatusCode = (int) statusCode
        };
    }

    public static object GenerateResponseObject<T>(HttpStatusCode statusCode, string message, T body = default!, string error = null!)
    {
        return new ApiResponse<T>
        {
            Code = (int) statusCode,
            Message = message,
            Body = body,
            Error = error
        };
    }

    public static IActionResult Ok<T>(T body, string message = "Solicitud exitosa")
    {
        return GenerateResponse<object>(HttpStatusCode.OK, message, body!);
    }

    public static IActionResult Created<T>(T body, string message = "Recurso creado exitosamente")
    {
        return GenerateResponse<object>(HttpStatusCode.Created, message, body!);
    }

    public static IActionResult BadRequest(string message = "Solicitud incorrecta", object errors = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.BadRequest, message, default!, errors);
    }

    public static IActionResult Unauthorized(string message = "No autorizado", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.Unauthorized, message, default!, error);
    }

    public static IActionResult Forbidden(string message = "Prohibido", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.Forbidden, message, default!, error);
    }

    public static object Unauthorized()
    {
        return GenerateResponseObject<object>(
            HttpStatusCode.Unauthorized, 
            "Token inválido o no proporcionado");
    }

    public static IActionResult NotFound(string message = "Recurso no encontrado")
    {
        return GenerateResponse<object>(HttpStatusCode.NotFound, message, default!);
    }

    public static IActionResult Conflict(string message = "Se produjo un conflicto", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.Conflict, message, default!, error);
    }

    public static IActionResult InternalServerError(string message = "Error Interno del Servidor", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.InternalServerError, message, default!, error);
    }
}