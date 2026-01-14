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
            .MapPost(Register, "register");
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

    public async Task<IResult> Register(CreateUserDto model, IdentityService identityService)
    {
        var result = await identityService.RegisterUserAsync(model);

        if (!result.Succeeded)
        {
            return Results.BadRequest(result.Errors);
        }

        return Results.Ok("User registered successfully.");
    }
}
