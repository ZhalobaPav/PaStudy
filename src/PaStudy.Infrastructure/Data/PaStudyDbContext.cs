using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection;
using PaStudy.Infrastructure.Models;

namespace PaStudy.Infrastructure.Data;

public class PaStudyDbContext: IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public PaStudyDbContext(DbContextOptions<PaStudyDbContext> options): base(options)
    {
        
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}