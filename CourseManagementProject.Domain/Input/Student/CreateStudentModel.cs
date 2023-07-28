using CourseManagementProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Domain.Input.Student
{
    public class CreateStudentModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public Subject Subject { get; set; }

    }
}
