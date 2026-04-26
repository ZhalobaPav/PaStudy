namespace PaStudy.Core.Helpers.FilterObjects.Submissions;

public record SubmissionFilter: BaseFilterRequest
{
    public int AssignmentId { get; set; }
}
