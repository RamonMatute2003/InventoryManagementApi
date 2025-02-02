using InventoryManagement.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InventoryManagement.Api.DTOs;
using InventoryManagement.Api.Helpers;

namespace InventoryManagement.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var token = await authService.AuthenticateAsync(request.Username, request.Password);

        if(token == null)
        {
            return ApiResponseHelper.Unauthorized("Credenciales inválidas");
        }

        return ApiResponseHelper.Ok(new { Token = token }, "Inicio de sesión exitoso");
    }
}