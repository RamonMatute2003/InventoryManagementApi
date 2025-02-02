using InventoryManagement.Api.Filters;
using InventoryManagement.Api.Helpers;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Services;
using InventoryManagement.Persistence.Contexts;
using InventoryManagement.Persistence.Repositories;
using InventoryManagement.Shared.Enums;
using InventoryManagement.Shared.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IInventoryOutRepository, InventoryOutRepository>();
builder.Services.AddScoped<IInventoryOutService, InventoryOutService>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

var key = Encoding.UTF8.GetBytes(configuration[ "Jwt:Key" ]!);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration[ "Jwt:Issuer" ],
            ValidAudience = configuration[ "Jwt:Audience" ],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = "role",
            NameClaimType = "nameid"
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                context.HandleResponse();
                context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var response = ApiResponseHelper.Unauthorized();

                await context.Response.WriteAsJsonAsync(response);
            },

            OnForbidden = async context =>
            {
                context.Response.StatusCode = (int) HttpStatusCode.Forbidden;
                context.Response.ContentType = "application/json";

                var response = ApiResponseHelper.Unauthorized();

                await context.Response.WriteAsJsonAsync(response);
            }
        };
    });

builder.Services.AddControllers();
builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ValidationFilter());
    options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventory Management API", Version = "v1" });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT en el formato: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next();

        if(!context.Response.HasStarted && context.Response.StatusCode == (int) HttpStatusCode.BadRequest)
        {
            context.Response.ContentType = "application/json";
            var problemDetails = await context.Request.ReadFromJsonAsync<ValidationProblemDetails>();

            var response = ApiResponseHelper.BadRequest("Error de validación", problemDetails?.Errors!);
            await context.Response.WriteAsJsonAsync(response);
        }
    } catch(Exception ex)
    {
        if(!context.Response.HasStarted)
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var response = ApiResponseHelper.InternalServerError("Error interno del servidor", ex.Message);
            await context.Response.WriteAsJsonAsync(response);
        }
    }
});

if(app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0.0");
        options.RoutePrefix = string.Empty;
    });

    var serverAddress = "https://localhost:5001";
    try
    {
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = serverAddress,
            UseShellExecute = true
        });
    } catch
    {
        Console.WriteLine($"No se pudo abrir automáticamente: {serverAddress}");
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();