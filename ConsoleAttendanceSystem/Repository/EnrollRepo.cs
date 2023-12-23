using ConsoleAttendanceSystem.Entities;
using ConsoleAttendanceSystem.EntityFramework;
using ConsoleAttendanceSystem.Validation;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Repository
{
    internal class EnrollRepo
    {
        TrainingDbContext context = new TrainingDbContext();
        string IdGenerator()
        {
            TrainingDbContext context = new TrainingDbContext();
            Enroll enr = context.Enrolls.OrderBy(x => x.EnrollId).LastOrDefault();
            if (enr == null) { return "E-001"; }
            string[] idValue = enr.EnrollId.Split('-');
            int number = Convert.ToInt32(idValue[1]);
            string newId = "E-" + (++number).ToString("d3");
            return newId;
        }
        string StringInput() //color coding for inputs
        {
            string input;
            Console.ForegroundColor = ConsoleColor.Green;
            input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            return input;
        }
        public string GetTeachersName(string id)
        {
            List<Teacher> teachers = context.Teachers.ToList();
            foreach (Teacher teach in teachers)
            {
                if(teach.TeacherId==id)
                {
                    return teach.Name;
                }
            }
            return string.Empty;
        }
        public string GetCourseTitle(string id)
        {
            List<Course> course = context.Courses.ToList();
            foreach (Course crs in course)
            {
                if (crs.CourseId == id)
                {
                    return crs.CourseName;
                }
            }
            return string.Empty;
        }
        bool ValidateInsertion(string sid,string cid)
        {
            Student student1 = context.Students.Where(x => x.StudentId == sid).FirstOrDefault();
            Course course = context.Courses.Where(x => x.CourseId == cid).FirstOrDefault();
            Enroll enroll = context.Enrolls.Where(x => x.StudentId == sid && x.CourseId == cid).FirstOrDefault();
            if(enroll!= null) { return false; }
            if (student1==null || course == null)
            {
                return false;
            }
            return true;
        }
        void GetAllEnrolls()
        {
            //StringBuilder studentList = new StringBuilder();
            //studentList.Append("Enroll Id \t Course Id \t Course Title \t Teacher Id \t Teacher Name \t Student Id\n");
            var table = new ConsoleTable("Enroll Id","Course Id","Course Title","Teacher Id","Teacher Name","Student Id");
            List<Enroll> enrolls = context.Enrolls.ToList();
            if (enrolls != null || enrolls.Count!=0) 
            {
                foreach (Enroll enroll in enrolls)
                {
                    table.AddRow(enroll.EnrollId, enroll.CourseId, GetCourseTitle(enroll.CourseId), enroll.TeacherId, GetTeachersName(enroll.TeacherId), enroll.StudentId);
                    // studentList.Append(enroll.EnrollId + "\t\t" + enroll.CourseId + "\t\t" + GetCourseTitle(enroll.CourseId) + "\t\t" + enroll.TeacherId + "\t\t" + GetTeachersName(enroll.TeacherId) + "\t\t" + enroll.StudentId + "\n");
                }
            }
            // Console.WriteLine(studentList);
            table.Write();
        }

        public bool EnrollCRUD(string key)
        {
            Console.Clear();
            List<Enroll> enrolls = context.Enrolls.ToList();
            Console.WriteLine("All Enrolments...");
            GetAllEnrolls();
            
            if (key == "e")//new student enroll
            {
                //Console.WriteLine("\n\n\nCourse List\n");
                //CourseRepo courseRepo = new CourseRepo();
                //courseRepo.GetAllCourses();
                //Console.WriteLine("\n\n\nStudents List\n");
                //StudentRepo studentRepo = new StudentRepo();
                //studentRepo.GetAllStudents();
                CourseRepo courseRepo = new CourseRepo();
                Console.WriteLine("All Courses...");
                courseRepo.GetAllCourses();
                Enroll enroll = new Enroll();
                enroll.EnrollId = IdGenerator();
                Console.WriteLine("Insert New Enroll");
                Console.WriteLine("Student Id:" + enroll.EnrollId + "  (Auto generated)");
                Console.Write("Course Id : ");
                enroll.CourseId = StringInput();
                Console.Write("Student Id: ");
                enroll.StudentId = StringInput();
                if (!ValidateInsertion(enroll.StudentId,enroll.CourseId))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Id or Duplicate Insertion..Press any key to Try Again");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press Escape to Cancel");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        return true;
                    }
                    EnrollCRUD("e");
                }
                else
                {
                    //enroll = context.Enrolls.Where(x => x.CourseId == enroll.CourseId).FirstOrDefault();
                    var tid=context.Courses.Where(x=>x.CourseId== enroll.CourseId).FirstOrDefault();
                    enroll.TeacherId = tid.TeacherId;
                    enroll.EnrollId=IdGenerator();
                    context.Enrolls.Add(enroll);
                    context.SaveChanges();
                    Console.WriteLine("Course Enrolled Successfully..\n\nPress any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    return true;
                }
            }
            else if (key == "j")//Delete enrollment
            {
                Console.Write("Enroll Id: ");
                string id = StringInput();
                foreach (Enroll enroll in enrolls)
                {
                    if (enroll.EnrollId == id)
                    {
                        Enroll en = context.Enrolls.Where(x => x.EnrollId == id).FirstOrDefault();
                        context.Enrolls.Remove(en);
                        context.SaveChanges();
                        Console.WriteLine("Enrollment Deleted Successfully..\n\nPress any key to continue");
                        Console.ReadKey();
                        Console.Clear();
                        return true;
                    }
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Id..Press any key to Try Again");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press Escape to Cancel");
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    Console.Clear();
                    return true;
                }
                EnrollCRUD("j");
            }
            return true;
        }
    }
}
