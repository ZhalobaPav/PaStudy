using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Reponses;

namespace PaStudy.Core.Interfaces.Service;

public interface IAssignmentService
{
    Task<BaseResponse<Assignment>> CreateAssignmentAsync(CreateAssignmentDto dto, string userId);
}
