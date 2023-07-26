using CourseManagementProject.Domain.Enums;


namespace CourseManagementProject.Domain.DomainModels
{
    public class Tutor : BaseEntity
    {
        public Subject Subject { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }



        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }


    }
}
