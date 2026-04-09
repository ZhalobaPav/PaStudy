using PaStudy.Core.Entities.Assignments.Submission;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;

namespace PaStudy.Infrastructure.Repositories;

public class SubmissionRepository: ISubmissionRepository
{
    private readonly PaStudyDbContext _dbContext;

    public SubmissionRepository(PaStudyDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateSubmission(Submission submission)
    {
        if(submission == null)
        {
            throw new ArgumentNullException(nameof(submission));
        }

        await _dbContext.Set<Submission>().AddAsync(submission);
        await _dbContext.SaveChangesAsync();
    }
}
