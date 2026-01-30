using PaStudy.Core.Helpers.DTOs.Assignment;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Section;

public class SectionDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
    public int CourseId { get; set; }
    public ImmutableArray<AssignmentDto> Assignments { get; set; } = ImmutableArray<AssignmentDto>.Empty;
}
