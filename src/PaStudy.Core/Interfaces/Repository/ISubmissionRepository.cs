using PaStudy.Core.Entities.Assignments.Submission;

namespace PaStudy.Core.Interfaces.Repository;

public interface ISubmissionRepository
{
    Task CreateSubmission(Submission submission);
}
