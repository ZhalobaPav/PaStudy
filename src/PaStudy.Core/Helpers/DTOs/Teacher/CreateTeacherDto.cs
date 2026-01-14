namespace PaStudy.Core.Helpers.DTOs.Teacher;

public class CreateTeacherDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public int GroupId { get; set; }
    public DateTime DateOfBirth { get; set; }
}
