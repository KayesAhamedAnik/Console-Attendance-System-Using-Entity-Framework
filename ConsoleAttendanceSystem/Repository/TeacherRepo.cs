using ConsoleAttendanceSystem.Entities;
using ConsoleAttendanceSystem.EntityFramework;
using ConsoleAttendanceSystem.Validation;
using ConsoleTables;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Repository
{
    internal class TeacherRepo
    {
        TrainingDbContext context = new TrainingDbContext();
        string IdGenerator()
        {
            
            Teacher tch = context.Teachers.OrderBy(x => x.TeacherId).LastOrDefault();
            if (tch == null) { return "T-001"; }
            string[] idValue = tch.TeacherId.Split('-');
            int number = Convert.ToInt32(idValue[1]);
            string newId = "T-" + (++number).ToString("d3");
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
        void GetAllTeacher()
        {
            //StringBuilder teacherList = new StringBuilder();
            //teacherList.Append("Teacher Id" + "\t" + "Name\n");
            var table = new ConsoleTable("Teacher Id","Name");
            
            List<Teacher> Teachers = context.Teachers.ToList();
            if(Teachers.Count!=0 || Teachers != null)
            {
                foreach (Teacher teacher in Teachers)
                {
                    //teacherList.Append(teacher.TeacherId + "\t\t" + teacher.Name + "\n");
                    table.AddRow(teacher.TeacherId, teacher.Name);
                }
            }
            table.Write();
            //Console.WriteLine(teacherList);
        }
        public bool TeacherCRUD(string key)
        {
            Console.Clear();
            GetAllTeacher();
            List<Teacher> Teachers = context.Teachers.ToList();
            if (key == "c")//insert new teacher
            {
                Teacher teacher = new Teacher();
                teacher.TeacherId = IdGenerator();
                Console.WriteLine("Insert New Teacher..");
                Console.WriteLine("Teacher Id:" + teacher.TeacherId + "  (Auto generated)");
                Console.Write("Name: ");
                teacher.Name = StringInput();
                Console.Write("Password: ");
                teacher.Password = StringInput();
                Validate validate = new Validate();
                if (!validate.IsValidName(teacher.Name) || !validate.IsPasswordValid(teacher.Password))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Name or Pass..Press any key to Try Again");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press Escape to Cancel");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        return true;
                    }
                    TeacherCRUD("c");


                }
                else
                {
                    context.Teachers.Add(teacher);
                    context.SaveChanges();
                    Console.WriteLine("Teacher Added Successfully..\n\nPress any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    return true;
                }
            }
            else if (key == "h")//update name or pass
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Update Teacher Info\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Update Name\t2.Update Password\t3. Delete\t4.Cancel");
                Console.Write("Insert Choice: ");
                string choice = StringInput();
                try
                {
                    if (Int32.Parse(choice) == 1 || Int32.Parse(choice) == 2 || Int32.Parse(choice) == 3 || Int32.Parse(choice) == 4)
                    {
                        if (Int32.Parse(choice) == 1)
                        {
                            Console.Write("Insert Teacher Id: ");
                            string id = StringInput();
                            foreach (Teacher teacher in Teachers)
                            {
                                if (teacher.TeacherId == id)
                                {
                                    Teacher teacher1 = context.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
                                    Console.Write("Insert updated Name: ");
                                    teacher1.Name = StringInput();
                                    Validate validate = new Validate();
                                    if (!validate.IsValidName(teacher.Name)) { break; }
                                    else
                                    {
                                        context.SaveChanges();
                                        Console.WriteLine("Updated Successfully..\n\nPress any key to continue");
                                        Console.ReadKey();
                                        Console.Clear();
                                        return true;
                                    }
                                }
                            }
                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Invalid Id or Name..Press any key to Try Again");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Press Escape to Cancel");
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                Console.Clear();
                                return true;
                            }
                            TeacherCRUD("h");
                        }
                        else if (Int32.Parse(choice) == 2)
                        {
                            Console.Write("Insert Teacher Id: ");
                            string id = StringInput();
                            foreach (Teacher teacher in Teachers)
                            {
                                if (teacher.TeacherId == id)
                                {
                                    Teacher teacher1 = context.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
                                    Console.Write("Insert updated Password: ");
                                    teacher1.Password = StringInput();
                                    Validate validate = new Validate();
                                    if (!validate.IsPasswordValid(teacher1.Password)) { break; }
                                    else
                                    {
                                        context.SaveChanges();
                                        Console.WriteLine("Updated Successfully..\n\nPress any key to continue");
                                        Console.ReadKey();
                                        Console.Clear();
                                        return true;
                                    }
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
                            TeacherCRUD("h");
                        }
                        else if (Int32.Parse(choice) == 3)
                        {
                            Console.Write("Insert Teacher Id: ");
                            string id = StringInput();
                            foreach (Teacher teacher in Teachers)
                            {
                                if (teacher.TeacherId == id)
                                {
                                    Teacher tch = context.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
                                    context.Teachers.Remove(tch);
                                    context.SaveChanges();
                                    Console.WriteLine("Teacher Deleted Successfully..\n\nPress any key to continue");
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
                            TeacherCRUD("h");
                        }
                        else if (Int32.Parse(choice) == 4)
                        {
                            Console.Clear();
                            return true;
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Choice..Press any key to Try Again");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Press Escape to Cancel");
                        if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                        {
                            Console.Clear();
                            return true;
                        }
                        TeacherCRUD("h");
                    }
                }
                catch
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Choice..Press any key to Try Again");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press Escape to Cancel");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }
        public void TeachersMenu(string tid)
        {
            Console.Clear();
            EnrolledCourses(tid);
            Console.WriteLine("Insert Enrolled Course Id to view Attendance Report.");
            Console.Write("Insert Choice:");
            string choice = StringInput();
            Validate val = new Validate();
            if(validateCourseId(choice,tid))
            {
                AttendanceRepo attendanceRepo = new AttendanceRepo();
                attendanceRepo.viewCourseAttendance(choice);
                Console.WriteLine("Press any key to go back");
                Console.WriteLine("Press Escape to Logout");
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    Environment.Exit(0);
                }
                Console.Clear();
                TeachersMenu(tid);
            }
            else
            {
                Console.WriteLine("Invalid Input");
                Console.ReadKey();
                Console.Clear();
                TeachersMenu(tid);
            }
        }
        bool validateCourseId(string cid,string tid)
        {
            Course course = context.Courses.Where(x => x.CourseId == cid && x.TeacherId == tid).FirstOrDefault();
            if (course != null && course.CourseId==cid)
            {
                return true;
            }
            return false;
        }
        void EnrolledCourses(string tid)
        {
            Console.WriteLine("Welcome to Teachers Panel\t\t Loggedin as: "+tid+"\n\n");
            var table = new ConsoleTable("CourseId", "Course Title", "Schedule", "No Of Students");
            List<Course> course = context.Courses.Where(x => x.TeacherId == tid).ToList();
            if(course.Count > 0)
            {
                
                string[] courses_ = new string[course.Count];
                int j = 0;
                foreach(Course course1 in course)
                {
                    courses_[j]=course1.CourseId;
                    j++;
                }
                for(int i=0;i<courses_.Length;i++)
                {
                    int count = context.Enrolls.Where(x => x.TeacherId == tid && x.CourseId==courses_[i]).Count();
                    Course enrolledcourse = context.Courses.Where(x => x.TeacherId == tid && x.CourseId == courses_[i]).FirstOrDefault();
                    table.AddRow(enrolledcourse.CourseId,enrolledcourse.CourseName,enrolledcourse.Schedule,count);
                }
                
            }
            else
            {
                table.AddRow("No Enrolled Courses");
            }
            table.Write();
        }
        //void AttendanceReport(string CourseId)
        //{

        //}
    }
}
