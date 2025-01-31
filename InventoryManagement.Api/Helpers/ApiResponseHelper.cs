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

    public static IActionResult Ok<T>(T body, string message = "✅ Request succeeded")
    {
        return GenerateResponse<object>(HttpStatusCode.OK, message, body);
    }

    public static IActionResult Created<T>(T body, string message = "✅ Resource created successfully")
    {
        return GenerateResponse<object>(HttpStatusCode.Created, message, body);
    }

    public static IActionResult BadRequest(string message = "❌ Bad request", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.BadRequest, message, default!, error);
    }

    public static IActionResult BadRequest(string message = "❌ Bad request", object errors = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.BadRequest, message, default!, errors);
    }

    public static IActionResult Unauthorized(string message = "🔒 Unauthorized", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.Unauthorized, message, default!, error);
    }

    public static IActionResult Forbidden(string message = "🚫 Forbidden", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.Forbidden, message, default!, error);
    }

    public static object Unauthorized()
    {
        return GenerateResponseObject<object>(
            HttpStatusCode.Unauthorized, 
            "Token inválido o no proporcionado");
    }

    public static object Forbidden()
    {
        return GenerateResponseObject<object>(
            HttpStatusCode.Forbidden,
            "No tienes permisos para acceder a este recurso");
    }

    public static IActionResult NotFound(string message = "🔎 Resource not found")
    {
        return GenerateResponse<object>(HttpStatusCode.NotFound, message, default!);
    }

    public static IActionResult Conflict(string message = "⚠️ Conflict occurred", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.Conflict, message, default!, error);
    }

    public static IActionResult InternalServerError(string message = "🔥 Internal server error", string error = null!)
    {
        return GenerateResponse<object>(HttpStatusCode.InternalServerError, message, default!, error);
    }
}