using PaStudy.Core.Helpers.DTOs.Teacher;
using System.Collections.Immutable;

namespace PaStudy.Core.Helpers.DTOs.Group;

public class GroupDto
{
    public int Id { get; set; }
    public string GroupNumber { get; set; }
    public string InstitutionNumber { get; set; }
    public int Year { get; set; }
    public string Faculty { get; set; }
    public string Speciality { get; set; }
    public TeacherDto Teacher { get; set; }
}
