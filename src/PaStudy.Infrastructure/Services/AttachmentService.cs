using PaStudy.Core.Entities.Attachments;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Infrastructure.Data;
using System.Collections.Immutable;

namespace PaStudy.Infrastructure.Services;

public class AttachmentService: IAttachmentService
{
    private readonly PaStudyDbContext _context;
    private readonly IAttachmentFactory _attachmentFactory;

    public AttachmentService(PaStudyDbContext context, IAttachmentFactory attachmentFactory)
    {
        _context = context;
        _attachmentFactory = attachmentFactory;
    }

    public async Task<ImmutableArray<Attachment>> UploadAttachmentsAsync(IEnumerable<CreateAttachmentDto> attachments)
    {
        var attachmentEntities = attachments.Select(dto => _attachmentFactory.CreateAttachment(dto)).ToList();
        _context.Set<Attachment>().AddRange(attachmentEntities);
        await _context.SaveChangesAsync();
        return attachmentEntities.ToImmutableArray();
    }
}
