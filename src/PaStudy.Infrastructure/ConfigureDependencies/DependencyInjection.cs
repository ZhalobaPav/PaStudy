using Microsoft.Extensions.DependencyInjection;
using PaStudy.Core.Interfaces.Factories;
using PaStudy.Core.Interfaces.Repository;
using PaStudy.Core.Interfaces.Service;
using PaStudy.Core.Services;
using PaStudy.Core.Services.Factories;
using PaStudy.Infrastructure.Repositories;
using PaStudy.Infrastructure.Services;

namespace PaStudy.Infrastructure.ConfigureDependencies;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
    {
        //Repositories
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<IStudentRepository, StudentRepository>();
        services.AddScoped<ITeacherRepository, TeacherRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IAssignmentRepository, AssignmentRepository>();

        //Services
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IdentityService>();
        services.AddScoped<DataSeederService>();
        services.AddScoped<IAssignmentService, AssignmentService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IAttachmentService, AttachmentService>();

        //Factories
        services.AddScoped<IAttachmentFactory, AttachmentFactory>();
        services.AddScoped<IAssignmentElementFactory, AssignmentFactory>();
        services.AddScoped<IQuestionFactory, QuestionFactory>();
        return services;
    }
}
