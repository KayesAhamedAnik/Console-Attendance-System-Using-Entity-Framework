using ConsoleAttendanceSystem.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Entities
{

    public class Attendance
    {
        public int AttendanceId { get; set; }
        public Student Student { get; set; }
        public Course Course { get; set; }
        public string Date { get; set; }
        public string CourseId { get; set; }
        public string StudentId { get; set; }
        public string status { get; set; }
    }
}
