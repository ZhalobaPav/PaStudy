using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Category;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Repositories;

public class CategoryRepository: ICategoryRepository
{
    private readonly PaStudyDbContext _dbContext;

    public CategoryRepository(PaStudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ImmutableArray<Category>> GetAllCategories(CancellationToken ct)
    {
        return await _dbContext.Set<Category>().AsNoTracking().ToImmutableArrayAsync(ct);
    }

    public async Task CreateCategoryAsync(CreateCategoryDto categoryDto, CancellationToken ct)
    {
        await _dbContext.Set<Category>().AddAsync(new Category { Name = categoryDto.Name });
        await _dbContext.SaveChangesAsync(ct);
    }
}
