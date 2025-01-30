using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Shared.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[ApiController]
[Route("api/users")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto userDto)
    {
        if(!ModelState.IsValid)
        {
            var errors = ModelState.Where(e => e.Value.Errors.Count > 0)
                .ToDictionary(
                    e => e.Key,
                    e => e.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                );

            return ApiResponseHelper.BadRequest("❌ Datos de entrada no válidos.", errors);
        }

        try
        {
            var createdUser = await _userService.CreateUserAsync(userDto);
            return ApiResponseHelper.Created(createdUser, "✅ Usuario creado exitosamente.");
        } catch(InvalidOperationException ex)
        {
            return ApiResponseHelper.Conflict(ex.Message);
        } catch(Exception ex)
        {
            return ApiResponseHelper.InternalServerError("Ocurrió un error al crear el usuario.", ex.Message);
        }
    }
}