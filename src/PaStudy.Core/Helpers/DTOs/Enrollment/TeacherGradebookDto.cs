namespace PaStudy.Core.Helpers.DTOs.Enrollment;

public record TeacherGradebookDto
{
    public int EnrollmentId { get; set; }
    public int StudentId { get; set; }
    public string StudentFullName { get; set; } = string.Empty;
    public string GroupName { get; set; } = string.Empty;
    public decimal? FinalGrade { get; set; }
    public double Progress { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
}
public record BulkEnrollmentResult
{
    public bool Success { get; set; } = false;
    public int EnrolledCount { get; set; } = 0;
    public int SkippedCount { get; set; } = 0;
    public string Message { get; set; } = string.Empty;
}