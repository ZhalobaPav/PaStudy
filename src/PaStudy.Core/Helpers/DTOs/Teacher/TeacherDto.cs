using PaStudy.Core.Helpers.DTOs.Group;

namespace PaStudy.Core.Helpers.DTOs.Teacher;

public class TeacherDto
{
    public int Id { get; set; } 
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public GroupDto Group { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
