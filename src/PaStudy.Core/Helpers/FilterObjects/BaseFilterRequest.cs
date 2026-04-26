namespace PaStudy.Core.Helpers.FilterObjects;

public record BaseFilterRequest
{
    public int? PageNumber { get; set; } = 1;
    public int? PageSize { get; set; } = 10;
    public string? SearchTerm { get; set; }
}
