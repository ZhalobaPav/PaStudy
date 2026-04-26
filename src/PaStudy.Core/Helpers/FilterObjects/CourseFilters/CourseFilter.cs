namespace PaStudy.Core.Helpers.FilterObjects.CourseFilters;

public record CourseFilter: BaseFilterRequest
{
    public CourseQuantity CourseQuantity { get; set; } = CourseQuantity.All;
}

public enum CourseQuantity : byte 
{ 
    All = 0,
    Enrolled = 1,
}
