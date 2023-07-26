using CourseManagementProject.Domain.Enums;

namespace CourseManagementProject.Domain.DomainModels;

public abstract class BaseEntity
{
    public int Id { get; set; }
}


public class User : BaseEntity
{
    public string Username { get; set; }


    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }


    public UserRole UserRole { get; set; }
}

