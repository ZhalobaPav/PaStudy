namespace PaStudy.Core.Helpers.DTOs.Reponses;

public record PagedResponse<T>(List<T> Items, int TotalCount);