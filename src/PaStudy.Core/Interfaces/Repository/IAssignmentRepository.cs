using Microsoft.AspNetCore.Http;
using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Section;
using System.Collections.Immutable;

namespace PaStudy.Core.Interfaces.Repository;

public interface IAssignmentRepository
{
    Task<ImmutableArray<AssignmentDto>> GetAssignmentsAsync(int courseId, CancellationToken cancellationToken);
    Task<ImmutableArray<SectionDto>> GetSectionsAsync(int courseId, CancellationToken cancellationToken);
    Task<Assignment> CreateAsync(Assignment assignment, CancellationToken ct = default);
    Task<Section> CreateSectionAsync(Section section, CancellationToken ct = default);
    Task CreateAttachment(CreateAttachmentDto createAttachmentDto);
    Task<string> SaveFileAsync(IFormFile file);
}
