using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Section;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class AssignmentMapper
{
    public static Assignment ToAssignmentEntity(this CreateAssignmentDto createAssignmentDto)
    {
        return new Assignment
        {
            Title = createAssignmentDto.Title,
            Description = createAssignmentDto.Description,
            Attachments = createAssignmentDto.Attachments.Select((att) => att.ToAttachmentEntity()).ToList(),
            DueDate = createAssignmentDto.DueDate,
            MaxPoints = createAssignmentDto.MaxPoints,
        };
    }
    public static AssignmentDto ToAssignmentDto(this Assignment assignment)
    {
        return new AssignmentDto
        {
            Id = assignment.Id,
            Title = assignment.Title,
            Description = assignment.Description ?? string.Empty,
            DueDate = assignment.DueDate ?? DateTime.MinValue,
            Attachments = assignment.Attachments.Select(att => att.ToAttachmentDto()).ToImmutableArray(),
            MaxPoints = assignment.MaxPoints
        };
    }
    public static AttachmentDto ToAttachmentDto(this Attachment attachment)
    {
        return new AttachmentDto
        {
            FileName = attachment.FileName,
            FileUrl = attachment.FileUrl,
            ContentType = attachment.ContentType
        };
    }

    public static Attachment ToAttachmentEntity(this AttachmentDto attachmentDto)
    {
        return new Attachment
        {
            FileName = attachmentDto.FileName,
            FileUrl = attachmentDto.FileUrl,
            ContentType = attachmentDto.ContentType
        };
    }

    public static SectionDto ToSectionDto(this Section section)
    {
        return new SectionDto
        {
            Title = section.Title,
            Description = section.Description,
            Order = section.Order,
            CourseId = section.CourseId,
            Assignments = section.Assignments != null ? section.Assignments.Select(a => a.ToAssignmentDto()).ToImmutableArray() : ImmutableArray<AssignmentDto>.Empty
        };
    }

    public static Section ToSectionEntity(this CreateSectionDto section)
    {
        return new Section
        {
            Title = section.Title,
            Description = section.Description,
            CourseId = section.CourseId
        };
    }
}
