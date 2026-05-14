namespace PaStudy.Core.Helpers.DTOs.Users;

public record LoginUserDto(string Email, string Password);
public record GoogleLoginDto(string IdToken);