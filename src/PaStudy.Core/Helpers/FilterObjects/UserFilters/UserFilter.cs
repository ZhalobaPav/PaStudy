using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Core.Helpers.FilterObjects.UserFilters;

public class UserFilter
{
    public int? Take { get; set; }
    public int? Skip { get; set; }
    public string? SearchTerm { get; set; }
    public FilterUserProfile? FilterUserProfile { get; set; }
    public int? CourseId { get; set; }
    
}
