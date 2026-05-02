using Microsoft.AspNetCore.Http;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Section;
using System.Collections.Immutable;
using System.Security.Claims;

namespace PaStudy.Core.Interfaces.Repository;

public interface IAssignmentRepository
{
    Task<ImmutableArray<AssignmentDto>> GetAssignmentsAsync(int courseId, CancellationToken cancellationToken);
    Task<AssignmentDto> GetAssignmentByIdAsync(int assignmentId, CancellationToken cancellationToken, ClaimsPrincipal user);
    Task<ImmutableArray<SectionDto>> GetSectionsAsync(int courseId, CancellationToken cancellationToken, ClaimsPrincipal user);
    Task<Assignment> CreateAsync(Assignment assignment, CancellationToken ct = default);
    Task<Section> CreateSectionAsync(Section section, CancellationToken ct = default);
    Task AddAttachmentsToAssignment(ICollection<CreateAttachmentDto> createAttachmentDtoList, int assignmentId);
    Task<StudentQuizDto?> GetQuizForPassingAsync(int quizId, CancellationToken cancellation);
    Task<string> SaveFileAsync(IFormFile file);
}
