using PaStudy.Core.Helpers.DTOs.Category;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Models;
using PavStudy.API.Extensions;

namespace PavStudy.API.Endpoints;

public class Category : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this).MapGet(GetAllCategories).MapPost(CreateCategory);
    }
    public async Task<IResult> GetAllCategories(CancellationToken ct, ICategoryRepository categoryRepository)
    {
        try
        {
            var categories = await categoryRepository.GetAllCategories(ct);
            return Results.Ok(categories);
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex);
        }
    }
    public async Task<IResult> CreateCategory(CreateCategoryDto categoryDto, CancellationToken ct, ICategoryRepository categoryRepository)
    {
        try
        {
            await categoryRepository.CreateCategoryAsync(categoryDto, ct);
            return Results.Ok();
        }
        catch (Exception ex) 
        {
            return Results.BadRequest(new { error = ex.Message });
        }
    }
}
