using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using PaStudy.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using PaStudy.Infrastructure.Models;
using Microsoft.EntityFrameworkCore.Diagnostics;
using PaStudy.Infrastructure.Data.Interceptors;
namespace PaStudy.Infrastructure.ConfigureDependencies
{
    public static class AddInfrastructureDependency
    {
        public static IServiceCollection AddDbDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<PaStudyDbContext>((sp, options) =>
            {
                options.AddInterceptors(sp.GetService<ISaveChangesInterceptor>());  
                options.UseSqlServer(configuration.GetConnectionString("PaStudyConnection"),
                x => x.MigrationsAssembly("PaStudy.Infrastructure"));
            });
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptors>();
            return services;
        }
        public static IServiceCollection AddInfrastructureIdentity(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequiredLength = 6;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<PaStudyDbContext>()
                .AddDefaultTokenProviders();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
                    };
                });
            return services;
        }
    }
}
