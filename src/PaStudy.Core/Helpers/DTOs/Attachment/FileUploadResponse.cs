namespace PaStudy.Core.Helpers.DTOs.Attachment;

public record FileUploadResponse(string Url, string FileName, string ContentType, long FileSize, int? Width = null, int? Height = null);