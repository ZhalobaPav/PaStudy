using PaStudy.Core.Entities.Assignments;
using PaStudy.Core.Helpers.DTOs.Assignment;
using PaStudy.Core.Helpers.DTOs.Assignment.Quiz;
using PaStudy.Core.Helpers.Enums;

namespace PaStudy.Core.Interfaces.Factories;

public interface IAssignmentElementFactory
{
    Assignment CreateAssignment(CreateAssignmentDto createAssignmentDto);
}
