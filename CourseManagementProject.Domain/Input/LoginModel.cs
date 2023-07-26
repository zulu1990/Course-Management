
using CourseManagementProject.Domain.Enums;

namespace CourseManagementProject.Domain.Input;

public record LoginModel(string Username, string Password, UserRole Role);




