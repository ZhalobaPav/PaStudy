using PaStudy.Core.Entities.Assignments.Questions;

namespace PaStudy.Core.Interfaces.Repository;

public interface IQuestionRepository
{
    Task<Question?> GetByIdAsync(int id);
    Task<List<Question>> GetByIdsAsync(IEnumerable<int> ids);
}
