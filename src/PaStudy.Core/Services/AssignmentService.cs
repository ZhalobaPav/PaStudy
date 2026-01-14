using PaStudy.Core.Entities;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Reponses;
using PaStudy.Core.Helpers.Exceptions;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;

namespace PaStudy.Core.Services;

public class AssignmentService: IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ITeacherRepository _teacherRepository;

    public AssignmentService(IAssignmentRepository assignmentRepository, ITeacherRepository teacherRepository)
    {
        _assignmentRepository = assignmentRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<BaseResponse<Assignment>> CreateAssignmentAsync(CreateAssignmentDto dto, string userId)
    {
        var teacher = await _teacherRepository.GetByUserIdAsync(userId);
        if (teacher == null) BaseResponse<Assignment>.Failure("Teacher not found");

        var hasAccess = await _teacherRepository.CanTeacherManageCourse(teacher.Id, dto.CourseId);
        if(!hasAccess) BaseResponse<Assignment>.Failure("Teacher has no access to this course");

        var assignment = dto.ToAssignmentEntity();
        var result = await _assignmentRepository.CreateAsync(assignment);
        return BaseResponse<Assignment>.Success(result);
    }

}
