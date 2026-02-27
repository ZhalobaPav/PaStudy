namespace PaStudy.Core.Helpers.DTOs.Attachment;

public record CreateAttachmentDto
{
    public string FileName { get; set; } = string.Empty;
    public string FileUrl { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public ImageAttachmentInfo? ImageInfo { get; set; }
}

public record ImageAttachmentInfo(int Width, int Height);

public static class AttachmentContentTypes
{
    public const string JPEG = "image/jpeg";
    public const string PNG = "image/png";
    public const string GIF = "image/gif";

    public static readonly HashSet<string> ImageContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        JPEG,
        PNG,
        GIF
    };

    public static bool IsImageContentType(string contentType)
    {
        return ImageContentTypes.Contains(contentType);
    }

    public const string PDF = "application/pdf";
    public const string PLAIN = "text/plain";

    public static readonly HashSet<string> DocumentContentTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
    {
        PDF,
        PLAIN
    };

    public static bool IsDocumentContentType(string contentType)
    {
        return DocumentContentTypes.Contains(contentType);
    }
}