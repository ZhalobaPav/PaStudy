using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PaStudy.Core.Entities;
using PaStudy.Core.Entities.ConnectionEntities;
using PaStudy.Core.Helpers.Enums;
using PaStudy.Infrastructure.Data;
using PaStudy.Infrastructure.Models;

namespace PaStudy.Infrastructure.Services
{
    public class DataSeederService
    {
        private readonly PaStudyDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DataSeederService(PaStudyDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            // Порядок: Users -> Groups (без кураторів) -> Teachers -> назначення кураторів -> Courses -> Students -> Enrollments -> TeacherCourses
            await SeedGroupsAsync();
            await SeedTeachersAsync();
            await SeedCoursesAsync();
            await SeedStudentsAsync();
            await SeedEnrollmentsAsync();
            await SeedTeacherCoursesAsync();
        }

        // -------------------------
        // GROUPS (без кураторів спочатку)
        // -------------------------
        private async Task SeedGroupsAsync()
        {
            var groupsSet = _context.Set<Group>();

            if (await groupsSet.AnyAsync()) return;

            var groups = new[]
            {
                new Group
                {
                    GroupNumber = "CS-101",
                    InstitutionNumber = "INST-001",
                    Year = 1,
                    Faculty = "Computer Science",
                    Speciality = "Software Engineering"
                },
                new Group
                {
                    GroupNumber = "CS-102",
                    InstitutionNumber = "INST-001",
                    Year = 1,
                    Faculty = "Computer Science",
                    Speciality = "Computer Engineering"
                },
                new Group
                {
                    GroupNumber = "MATH-201",
                    InstitutionNumber = "INST-001",
                    Year = 2,
                    Faculty = "Mathematics",
                    Speciality = "Applied Mathematics"
                },
                new Group
                {
                    GroupNumber = "PHYS-301",
                    InstitutionNumber = "INST-001",
                    Year = 3,
                    Faculty = "Physics",
                    Speciality = "Theoretical Physics"
                }
            };

            await groupsSet.AddRangeAsync(groups);
            await _context.SaveChangesAsync();
        }
        private async Task SeedTeachersAsync()
        {
            var teachersSet = _context.Set<Teacher>();
            if (await teachersSet.AnyAsync()) return;

            var predefinedTeachers = new[]
            {
        new { Id = "teacher_1", FirstName = "John", LastName = "Smith", MiddleName = "A." },
        new { Id = "teacher_2", FirstName = "Sarah", LastName = "Johnson", MiddleName = "B." },
        new { Id = "teacher_3", FirstName = "Mike", LastName = "Brown", MiddleName = "C." },
        new { Id = "teacher_4", FirstName = "Anna", LastName = "Wilson", MiddleName = "D." }
    };
            var groups = await _context.Set<Group>().ToListAsync();


            for (int idx = 0; idx < predefinedTeachers.Length; idx++)
            {
                var t = predefinedTeachers[idx];
                teachersSet.Add(new Teacher
                {
                    UserId = t.Id,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    MiddleName = t.MiddleName,
                    GroupId = groups[idx % groups.Count].Id  // циклічно призначаємо групу
                });
            }

            await _context.SaveChangesAsync();
        }

        // -------------------------
        // COURSES
        // -------------------------
        private async Task SeedCoursesAsync()
        {
            var coursesSet = _context.Set<Course>();

            if (await coursesSet.AnyAsync()) return;

            var courses = new[]
            {
                new Course { Title = "Introduction to Programming" },
                new Course { Title = "Data Structures and Algorithms" },
                new Course { Title = "Database Systems" },
                new Course { Title = "Web Development" },
                new Course { Title = "Calculus I" },
                new Course { Title = "Linear Algebra" },
                new Course { Title = "Object-Oriented Programming" },
                new Course { Title = "Software Engineering" }
            };

            await coursesSet.AddRangeAsync(courses);
            await _context.SaveChangesAsync();
        }

        private async Task SeedStudentsAsync()
        {
            var studentsSet = _context.Set<Student>();
            if (await studentsSet.AnyAsync()) return;

            var groups = await _context.Set<Group>().ToListAsync();
            if (!groups.Any()) return;

            var lastNames = new[] { "Doe", "Smith", "Johnson", "Brown", "Wilson", "Taylor", "Anderson", "Thomas" };
            var firstNames = new[] { "John", "Jane", "Michael", "Emily", "David", "Sarah", "Chris", "Amanda" };
            var rnd = new Random();
            var students = new List<Student>();

            for (int i = 1; i <= 30; i++)
            {
                var g = groups[rnd.Next(groups.Count)];
                var userId = $"student_{i}";

                // просто створюємо студентів з існуючими UserId
                students.Add(new Student
                {
                    UserId = userId,
                    FirstName = firstNames[rnd.Next(firstNames.Length)],
                    LastName = lastNames[rnd.Next(lastNames.Length)],
                    MiddleName = $"{Convert.ToChar('A' + rnd.Next(0, 26))}.",
                    DateOfBirth = new DateTime(2000, 1, 1).AddDays(i * 30),
                    GroupId = g.Id
                });
            }

            await studentsSet.AddRangeAsync(students);
            await _context.SaveChangesAsync();
        }

        // -------------------------
        // ENROLLMENTS
        // -------------------------
        private async Task SeedEnrollmentsAsync()
        {
            var enrollmentsSet = _context.Set<Enrollment>();

            if (await enrollmentsSet.AnyAsync()) return;

            var students = await _context.Set<Student>().ToListAsync();
            var courses = await _context.Set<Course>().ToListAsync();
            if (!students.Any() || !courses.Any()) return;

            var rnd = new Random();
            var enrollments = new List<Enrollment>();

            foreach (var student in students)
            {
                var take = rnd.Next(3, Math.Min(7, courses.Count + 1));
                var selected = courses.OrderBy(_ => rnd.Next()).Take(take);
                foreach (var course in selected)
                {
                    enrollments.Add(new Enrollment
                    {
                        StudentId = student.Id,
                        CourseId = course.Id,
                        Progress = Math.Round(rnd.NextDouble() * 100, 2),
                        FinalGrade = rnd.Next(60, 101),
                        Status = EnrollmentStatus.Active
                    });
                }
            }

            await enrollmentsSet.AddRangeAsync(enrollments);
            await _context.SaveChangesAsync();
        }

        // -------------------------
        // TEACHER–COURSE LINKS
        // -------------------------
        private async Task SeedTeacherCoursesAsync()
        {
            var set = _context.Set<TeacherCourses>();

            if (await set.AnyAsync()) return;

            var teachers = await _context.Set<Teacher>().ToListAsync();
            var courses = await _context.Set<Course>().ToListAsync();

            if (teachers.Count < 4 || courses.Count < 8)
            {
                // якщо даних недостатньо — просто звʼяжемо наявні записи без падіння
                var linksSimple = new List<TeacherCourses>();
                for (int i = 0; i < teachers.Count; i++)
                {
                    var course = courses.ElementAtOrDefault(i);
                    if (course != null)
                        linksSimple.Add(new TeacherCourses { TeacherId = teachers[i].Id, CourseId = course.Id });
                }

                if (linksSimple.Any())
                {
                    await set.AddRangeAsync(linksSimple);
                    await _context.SaveChangesAsync();
                }

                return;
            }

            var links = new[]
            {
                new TeacherCourses { TeacherId = teachers[0].Id, CourseId = courses[0].Id },
                new TeacherCourses { TeacherId = teachers[0].Id, CourseId = courses[2].Id },
                new TeacherCourses { TeacherId = teachers[1].Id, CourseId = courses[1].Id },
                new TeacherCourses { TeacherId = teachers[1].Id, CourseId = courses[6].Id },
                new TeacherCourses { TeacherId = teachers[2].Id, CourseId = courses[3].Id },
                new TeacherCourses { TeacherId = teachers[2].Id, CourseId = courses[7].Id },
                new TeacherCourses { TeacherId = teachers[3].Id, CourseId = courses[4].Id },
                new TeacherCourses { TeacherId = teachers[3].Id, CourseId = courses[5].Id }
            };

            await set.AddRangeAsync(links);
            await _context.SaveChangesAsync();
        }
    }
}
