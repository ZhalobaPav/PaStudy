using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Attachment;
using PaStudy.Core.Helpers.DTOs.Reponses;
using PaStudy.Core.Helpers.DTOs.Section;

namespace PaStudy.Core.Interfaces.Service;

public interface IAssignmentService
{
    Task<BaseResponse<Assignment>> CreateAssignmentAsync(CreateAssignmentDto dto, string userId);
    Task<BaseResponse<Section>> CreateSectionAsync(CreateSectionDto dto, string userId);
}
