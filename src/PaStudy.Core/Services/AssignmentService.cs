using PaStudy.Core.Entities;
using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Reponses;
using PaStudy.Core.Helpers.DTOs.Section;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;

namespace PaStudy.Core.Services;

public class AssignmentService: IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly ITeacherRepository _teacherRepository;
    private readonly IAttachmentService _attachmentService;
    private readonly IAssignmentElementFactory _assignmentFactory;

    public AssignmentService(
        IAssignmentRepository assignmentRepository, 
        ITeacherRepository teacherRepository, 
        IAttachmentService attachmentService, 
        IAssignmentElementFactory assignmentFactory)
    {
        _assignmentRepository = assignmentRepository;
        _teacherRepository = teacherRepository;
        _attachmentService = attachmentService;
        _assignmentFactory = assignmentFactory;
    }

    public async Task<BaseResponse<Assignment>> CreateAssignmentAsync(CreateAssignmentDto dto, string userId)
    {
        var teacher = await _teacherRepository.GetByUserIdAsync(userId);
        if (teacher == null) BaseResponse<Assignment>.Failure("Teacher not found");

        var assignment = _assignmentFactory.CreateAssignment(dto);
        var result = await _assignmentRepository.CreateAsync(assignment);
        return BaseResponse<Assignment>.Success(result);
    }

    public async Task<BaseResponse<Section>> CreateSectionAsync(CreateSectionDto dto, string userId)
    {
        var teacher = await _teacherRepository.GetByUserIdAsync(userId);
        if (teacher == null) BaseResponse<Section>.Failure("Teacher not found");

        var hasAccess = await _teacherRepository.CanTeacherManageCourse(teacher.Id, dto.CourseId);
        if (!hasAccess) BaseResponse<Section>.Failure("Teacher has no access to this course");

        var section = dto.ToSectionEntity();
        var result = await _assignmentRepository.CreateSectionAsync(section);
        return BaseResponse<Section>.Success(result);
    }
    public async Task<BaseResponse<List<Section>>> BulkCreateSectionsAsync(BulkCreateSectionDto dto, string userId)
    {
        // 1. Отримуємо викладача та обов'язково робимо return у разі помилки
        var teacher = await _teacherRepository.GetByUserIdAsync(userId);
        if (teacher == null)
            return BaseResponse<List<Section>>.Failure("Teacher not found");

        // 2. Перевіряємо доступ до курсу (один раз на весь масив)
        var hasAccess = await _teacherRepository.CanTeacherManageCourse(teacher.Id, dto.CourseId);
        if (!hasAccess)
            return BaseResponse<List<Section>>.Failure("Teacher has no access to this course");

        if (dto.Sections == null || !dto.Sections.Any())
            return BaseResponse<List<Section>>.Failure("Sections list cannot be empty");

        var createdSections = new List<Section>();

        foreach (var item in dto.Sections)
        {
            var section = new Section
            {
                Title = item.Title,
                Description = item.Description,
                CourseId = dto.CourseId
            };

            var result = await _assignmentRepository.CreateSectionAsync(section);
            createdSections.Add(result);
        }

        return BaseResponse<List<Section>>.Success(createdSections);
    }
}
