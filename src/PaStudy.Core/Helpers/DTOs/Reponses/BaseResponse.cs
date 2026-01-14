namespace PaStudy.Core.Helpers.DTOs.Reponses;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public bool Succeeded { get; set; }
    public string Message { get; set; }
    public List<string>? Errors { get; set; }
    public static BaseResponse<T> Success(T data, string? message = null)
        => new() { Succeeded = true, Data = data, Message = message };

    public static BaseResponse<T> Failure(List<string> errors, string? message = null)
        => new() { Succeeded = false, Errors = errors, Message = message };

    public static BaseResponse<T> Failure(string error)
        => new() { Succeeded = false, Errors = new List<string> { error } };
}
