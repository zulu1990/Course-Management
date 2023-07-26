using CourseManagementProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Domain.DomainModels
{
    public class Student : BaseEntity
    {
        public ICollection<Subject> Subjects { get; set; }

    }
}
