namespace PaStudy.Core.Helpers.DTOs.Course.Note;

public record NoteDto
{
    public decimal Percentage { get; set; }
    public decimal? Grade { get; set; }
    public string TeacherFeadback { get; set; } 
    public NoteAssignmentInfo AssignmentInfo { get; set; }
}

public record struct NoteAssignmentInfo(int MaxPoints, string Name, int Id);