
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;

namespace PaStudy.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly PaStudyDbContext _dbContext;

    public QuestionRepository(PaStudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Question>> GetByIdsAsync(IEnumerable<int> ids)
    {
        return await _dbContext.Set<Question>()
            .Include(q => (q as ChoiceQuestion).Options)
            .Include(q => (q as MatchingQuestion).Pairs)
            .Include(q => q.Attachments)
            .Where(q => ids.Contains(q.Id))
            .ToListAsync();
    }

    public async Task<Question?> GetByIdAsync(int id)
    {
        return await _dbContext.Set<Question>()
            .Include(q => (q as ChoiceQuestion).Options)
            .Include(q => (q as MatchingQuestion).Pairs)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}
