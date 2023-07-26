using CourseManagementProject.Domain.DomainModels;

namespace CourseManagementProject.Application.Repositories;

public interface IStudentRepository
{
    Task<User> GetStudentByUsername(string username);
    Task RegisterStudent(User user);
}
