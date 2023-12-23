using ConsoleAttendanceSystem.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAttendanceSystem.Entities;
using ConsoleTables;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAttendanceSystem.Repository
{
    internal class AttendanceRepo
    {
        TrainingDbContext Context=new TrainingDbContext();
        Attendance atn = new Attendance();
         public bool InsertAttendance(string cid,string sid)
        {
            

            List<Attendance> attendanceList = Context.Attendance.Where(x=>x.CourseId==cid && x.StudentId==sid).ToList();
            int count = 0;
            foreach(Attendance atn in attendanceList)
            {
                if(atn.Date.Contains(DateTime.Now.ToString("MM/dd/yyyy")))
                {
                    count++;
                }
            }
            if(count== 0)
            {
                var attendance = new Attendance() { CourseId = cid, StudentId = sid, status = "P", Date = System.DateTime.Now.ToString("dddd").Substring(0, 3) + " " + DateTime.Now.ToString("MM/dd/yyyy hh:mm tt") };
                Context.Attendance.Add(attendance);
                Context.SaveChanges();
                return true;
            }
            return false;
        }
        public void viewStudentAttendance(string cid,string sid)
        {
            List<Attendance> attendances = new List<Attendance>();
            attendances=Context.Attendance.Where(x=>x.StudentId==sid && x.CourseId == cid).ToList();
            var table = new ConsoleTable("Course Id", "Student Id", "Date", "Attendance");
            if (attendances != null || attendances.Count != 0)
            {
                foreach (Attendance attendance in attendances)
                {
                    //string atn = null;
                   // if(attendance.status==1 || attendance.status == 0) { atn = "P"; } else { atn = "X"; }
                    table.AddRow(attendance.CourseId,attendance.StudentId,attendance.Date,attendance.status);
                    
                }
            }
            table.Write();
        }
        public void viewCourseAttendance(string cid)
        {
            List<Attendance> attendances = new List<Attendance>();
            attendances = Context.Attendance.Where(x => x.CourseId == cid).ToList();
            var table = new ConsoleTable("Course Id", "Student Id", "Date", "Attendance");
            if (attendances != null || attendances.Count != 0)
            {
                foreach (Attendance attendance in attendances)
                {
                    string atn = null;
                   // if (attendance.status == 1 || attendance.status == 0) { atn = "P"; } else { atn = "X"; }
                    table.AddRow(attendance.CourseId, attendance.StudentId, attendance.Date,attendance.status);

                }
            }
            table.Write();
        }

    }
}
