using CourseManagementProject.Domain;
using CourseManagementProject.Domain.DomainModels;
using CourseManagementProject.Domain.Enums;

namespace CourseManagementProject.Application.Repositories;

public interface IStudentRepository
{
    Task<Result<Student>> GetStudentByUsername(string username);
    Task<Result> RegisterStudent(User user);
    Task<Result> UpadateStudentSubjects(List<Subject> subjects, int id);
}
