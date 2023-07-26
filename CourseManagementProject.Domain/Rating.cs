using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementProject.Domain
{
    public class Rating
    {
        public int LectureId { get; set; }


        public int StudentId { get; set; }
        public int Rate { get; set; }

        public string Comment { get; set; }
    }
}
