using CourseManagementProject.Domain;
using CourseManagementProject.Domain.DomainModels;

namespace CourseManagementProject.Application.Repositories;

public interface ITeachersRepository
{
    Task<Result<User>> GetTeacherByUsername(string username);
    Task<Result> RegisterTeacher(User user);
}
