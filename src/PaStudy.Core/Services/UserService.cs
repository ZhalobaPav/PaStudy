using PaStudy.Core.Helpers.DTOs.Users;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Core.Helpers.Extensions.MapperHelpers;
using PaStudy.Core.Helpers.FilterObjects.UserFilters;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;

namespace PaStudy.Core.Services;

public class UserService: IUserService
{
    private readonly IStudentRepository _studentRepository;
    private readonly ITeacherRepository _teacherRepository;

    public UserService(IStudentRepository studentRepository, ITeacherRepository teacherRepository)
    {
        _studentRepository = studentRepository;
        _teacherRepository = teacherRepository;
    }

    public async Task<ImmutableArray<UserProfileResponseDto>> GetUsers(UserFilter userFilter, CancellationToken cancellationToken)
    {
        async Task<IEnumerable<UserProfileResponseDto>> GetStudentsProfilesAsync()
        {
            var students = await _studentRepository.GetStudents(cancellationToken, userFilter);
            return students.Select(s => s.ToUserProfile());
        }

        async Task<IEnumerable<UserProfileResponseDto>> GetTeachersProfilesAsync()
        {
            var teachers = await _teacherRepository.GetTeachers(cancellationToken, userFilter);
            return teachers.Select(s => s.ToUserProfile());
        }
        var tasks = new List<Task<IEnumerable<UserProfileResponseDto>>>();
        userFilter.FilterUserProfile ??= FilterUserProfile.OnlyStudents;
        switch (userFilter.FilterUserProfile)
        {
            case FilterUserProfile.OnlyStudents:
                tasks.Add(GetStudentsProfilesAsync());
                break;
            case FilterUserProfile.OnlyTeachers:
                tasks.Add(GetTeachersProfilesAsync());
                break;
            case FilterUserProfile.Both:
                tasks.Add(GetStudentsProfilesAsync());
                tasks.Add(GetTeachersProfilesAsync());
                break;
        }
        var results = await Task.WhenAll(tasks);
        var combinedResults = results.SelectMany(r => r).ToImmutableArray();
        return combinedResults;
    }
    
}
