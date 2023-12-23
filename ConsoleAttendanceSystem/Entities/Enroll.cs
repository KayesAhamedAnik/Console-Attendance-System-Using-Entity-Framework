using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Entities
{
    public class Enroll
    {
        public string EnrollId { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        public Teacher Teacher { get; set; }
        public string CourseId { get; set; }
        public string TeacherId { get; set; }
        public string StudentId { get; set; }
        
    }
}
