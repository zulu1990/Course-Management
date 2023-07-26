using CourseManagementProject.Domain.DomainModels;

namespace CourseManagementProject.Application.Services;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
