using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Category;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository;

public interface ICategoryRepository
{
    Task<ImmutableArray<Category>> GetAllCategories(CancellationToken ct);
    Task CreateCategoryAsync(CreateCategoryDto categoryDto, CancellationToken ct);
}
