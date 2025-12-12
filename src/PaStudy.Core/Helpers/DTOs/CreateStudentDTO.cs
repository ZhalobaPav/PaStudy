namespace PaStudy.Core.Helpers.DTOs;

public class CreateStudentDTO
{
    public string UserId { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public DateTime DateOfBirth { get; set; }

    public int GroupId { get; set; }
}
