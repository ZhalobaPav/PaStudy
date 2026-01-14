using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;

namespace PaStudy.Core.Helpers.Extensions.MapperHelpers;

public static class AssignmentMapper
{
    public static Assignment ToAssignmentEntity(this CreateAssignmentDto createAssignmentDto)
    {
        return new Assignment
        {
            Title = createAssignmentDto.Title,
            Description = createAssignmentDto.Description,
            AttachmentUrl = createAssignmentDto.AttachmentUrl,
            DueDate = createAssignmentDto.DueDate,
            MaxPoints = createAssignmentDto.MaxPoints,
            CourseId = createAssignmentDto.CourseId
        };
    }
}
