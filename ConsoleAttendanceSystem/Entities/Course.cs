using ConsoleAttendanceSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Entities
{
    public class Course
    {
        public Teacher teacher { get; set; }
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string TeacherId { get; set; }
        public string Schedule { get; set; }
        public int Fees { get; set; }
        public IList<Enroll>EnrolledCourses { get; set; }
        public IList<Attendance> Attendances { get; set; }
    }
}
