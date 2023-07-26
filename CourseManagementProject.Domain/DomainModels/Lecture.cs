using CourseManagementProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Domain.DomainModels
{
    public class Lecture : BaseEntity
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public Subject Subject { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string VideoRecordLocation { get; set; }

        public ICollection<Rating> Ratings { get; set; }

        public ICollection<Student> Attendance { get; set; }

    }
}
