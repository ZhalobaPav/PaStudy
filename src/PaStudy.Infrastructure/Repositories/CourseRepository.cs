using PaStudy.Core.Entities;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Repositories;
public class CourseRepository: ICourseRepository
{
    private readonly PaStudyDbContext _context;

    public CourseRepository(PaStudyDbContext context)
    {
        _context = context;
    }

    public Task<ImmutableArray<Course>> GetCourses(CancellationToken cancellationToken)
    {
        return _context.Set<Course>().ToImmutableArrayAsync(cancellationToken);
    }
}

