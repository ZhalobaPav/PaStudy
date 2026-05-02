using PaStudy.Core.Helpers.DTOs.Course;
using PaStudy.Core.Helpers.DTOs.Identity;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Profile;

public record BaseProfileDto
{
    public string FullName { get; set; }    
    public string? Email { get; set; }
    public ImmutableArray<CourseDto> Courses { get; set; }
    public UserRole UserRole { get; set; }
}

