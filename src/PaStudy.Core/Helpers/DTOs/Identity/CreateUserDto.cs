namespace PaStudy.Core.Helpers.DTOs.Identity
{
    public class CreateUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public int GroupId { get; set; }
        public UserRole Role { get; set; } = UserRole.Student;
    }
}
