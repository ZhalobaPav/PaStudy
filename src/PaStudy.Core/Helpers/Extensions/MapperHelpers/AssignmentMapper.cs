using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.Assignments.Questions;
using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Section;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class AssignmentMapper
{
    public static AssignmentDto ToAssignmentDto(this Assignment assignment)
    {
        return new AssignmentDto
        {
            Id = assignment.Id,
            Title = assignment.Title,
            Description = assignment.Description ?? string.Empty,
            DueDate = assignment.DueDate ?? DateTime.MinValue,
            Attachments = assignment.Attachments.Select(att => att.ToAttachmentDto()).ToImmutableArray(),
            MaxPoints = assignment.MaxPoints,
            AssignmentType = assignment.AssignmentType
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
