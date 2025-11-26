using PaStudy.Core.Entities;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces
{
    public interface ICourseRepository
    {
        Task<ImmutableArray<Course>> GetCourses(CancellationToken cancellationToken);
    }
}
