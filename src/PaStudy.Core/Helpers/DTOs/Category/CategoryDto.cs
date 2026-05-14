namespace PaStudy.Core.Helpers.DTOs.Category;

public record CategoryBriefDto
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
}
