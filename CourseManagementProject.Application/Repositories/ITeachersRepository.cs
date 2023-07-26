using CourseManagementProject.Domain.DomainModels;

namespace CourseManagementProject.Application.Repositories;

public interface ITeachersRepository
{
    Task<User> GetTeacherByUsername(string username);
    Task RegisterTeacher(User user);
}
