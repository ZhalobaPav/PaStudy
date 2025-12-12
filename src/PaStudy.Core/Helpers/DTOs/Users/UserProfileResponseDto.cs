using PaStudy.Core.Helpers.DTOs.Group;
using PaStudy.Core.Helpers.DTOs.Identity;

namespace PaStudy.Core.Helpers.DTOs.Users;

public class UserProfileResponseDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public DateTime? DateOfBirth { get; set; } 
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public GroupDto Group { get; set; }
    public UserRole Role { get; set; }
}
