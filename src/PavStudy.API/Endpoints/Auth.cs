using Microsoft.AspNetCore.Identity;
using PaStudy.Core.Helpers.DTOs.Identity;
using PaStudy.Core.Helpers.DTOs.Users;
using PaStudy.Infrastructure.ConfigureDependencies;
using PaStudy.Infrastructure.Models;
using PaStudy.Infrastructure.Services;
using PavStudy.API.Extensions;

namespace PavStudy.API.Endpoints;

public class Auth : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapPost(Login, "login")
            .MapPost(Register, "register")
            .MapPost(GoogleLogin, "google-login")
            .MapPost(BulkRegister, "bulk-register");
    }
    

    public async Task<IResult> Login(LoginUserDto model, IdentityService identityService)
    {
        var result = await identityService.LoginAsync(model);

        if (result == null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(result);
    }

    public async Task<IResult> GoogleLogin(GoogleLoginDto model, IdentityService identityService)
    {
        var result = await identityService.GoogleLoginAsync(model);
        if (!result.Succeeded)
        {
            return Results.Unauthorized();
        }
        return Results.Ok(result);
    }

    public async Task<IResult> Register(CreateUserDto model, IdentityService identityService)
    {
        var result = await identityService.RegisterUserAsync(model);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        return Results.Ok("User registered successfully.");
    }

    public async Task<IResult> BulkRegister(List<CreateUserDto> models, IdentityService identityService)
    {
        if (models == null || !models.Any())
        {
            return Results.BadRequest("The user list cannot be empty.");
        }

        var successCount = 0;
        var errors = new List<object>();

        foreach (var model in models)
        {
            var result = await identityService.RegisterUserAsync(model);

            if (result.Succeeded)
            {
                successCount++;
            }
            else
            {
                errors.Add(new
                {
                    Email = model.Email,
                    Reasons = result.Errors.Select(e => e.Description).ToList()
                });
            }
        }

        return Results.Ok(new
        {
            Message = $"Bulk registration completed. Successfully registered: {successCount}/{models.Count}",
            FailedUsers = errors
        });
    }
}
