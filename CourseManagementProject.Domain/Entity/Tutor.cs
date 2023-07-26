using CourseManagementProject.Domain.Enums;


namespace CourseManagementProject.Domain.DomainModels;

public class Tutor : User
{
    public Subject Subject { get; set; }


}
