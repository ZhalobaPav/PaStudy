using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Core.Helpers.FilterObjects;

public class UserFilter
{
    public string? SearchTerm { get; set; }
    public FilterUserProfile? FilterUserProfile { get; set; }
}
