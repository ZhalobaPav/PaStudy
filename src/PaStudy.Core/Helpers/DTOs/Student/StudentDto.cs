using PaStudy.Core.Helpers.DTOs.Group;

namespace PaStudy.Core.Helpers.DTOs.Student;

public class StudentDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public GroupDto Group { get; set; }

}
