using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Entities.ConnectionEntities;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Section;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Extensions;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Repositories;

public class AssignmentRepository: IAssignmentRepository
{
    private readonly PaStudyDbContext _dbContext;
    private readonly IAttachmentFactory _attachmentFactory;

    public AssignmentRepository(PaStudyDbContext dbContext, IAttachmentFactory attachmentFactory)
    {
        _dbContext = dbContext;
        _attachmentFactory = attachmentFactory;
    }
    public async Task<Assignment> CreateAsync(Assignment assignment, CancellationToken ct = default)
    {
        var assignments = _dbContext.Set<Assignment>();
        await assignments.AddAsync(assignment, ct);
        await _dbContext.SaveChangesAsync(ct);
        return assignment;
    } 
    public async Task<Section> CreateSectionAsync(Section section, CancellationToken ct = default)
    {
        var sections = _dbContext.Set<Section>();
        int nextOrder = await sections
            .Where(s => s.CourseId == section.CourseId)
            .Select(s => (int?)s.Order)
            .MaxAsync() ?? 0;
        section.Order = nextOrder + 1;
        await sections.AddAsync(section, ct);
        await _dbContext.SaveChangesAsync(ct);
        return section;
    }
    public async Task<ImmutableArray<SectionDto>> GetSectionsAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Section>().Where(s => s.CourseId == courseId).Select(s => s.ToSectionDto())
            .ToImmutableArrayAsync(cancellationToken);
    }
    public async Task<ImmutableArray<AssignmentDto>> GetAssignmentsAsync(int courseId, CancellationToken cancellationToken)
    {
        return await _dbContext.Set<Assignment>().Where(a => a.Section.CourseId == courseId).Select(a => a.ToAssignmentDto())
            .ToImmutableArrayAsync(cancellationToken);
    }
    public async Task AddAttachmentsToAssignment(ICollection<CreateAttachmentDto> createAttachmentDtoList, int assignmentId)
    {
        var assignment = await _dbContext.Set<Assignment>()
        .Include(a => a.Attachments)
        .FirstOrDefaultAsync(a => a.Id == assignmentId);

        if (assignment == null) throw new NotFoundException("Assignment not found");
        var attachments = createAttachmentDtoList.Select(a => _attachmentFactory.CreateAttachment(a));
        foreach (var attachment in attachments)
        {
            assignment.Attachments.Add(attachment);
        }
        await _dbContext.SaveChangesAsync();
    }
    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "attachments");

        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/attachments/{fileName}";
    }
}
