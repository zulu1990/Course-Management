using CourseManagementProject.Domain.Enums;

namespace CourseManagementProject.Domain.Input;

public record RegisterModel(string Username, string Password, UserRole UserRole, Subject Subject);
