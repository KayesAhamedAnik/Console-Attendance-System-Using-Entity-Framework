using ConsoleAttendanceSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Entities
{
    public class Student
    {
        public string StudentId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public IList<Enroll> EnrolledCourses { get; set; }
        public IList<Attendance> Attendances { get; set; }
    }
}
