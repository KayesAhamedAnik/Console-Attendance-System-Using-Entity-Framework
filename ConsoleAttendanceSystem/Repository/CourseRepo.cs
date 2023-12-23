using ConsoleAttendanceSystem.Entities;
using ConsoleAttendanceSystem.EntityFramework;
using ConsoleAttendanceSystem.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTables;
namespace ConsoleAttendanceSystem.Repository
{
    internal class CourseRepo
    {
        TrainingDbContext context = new TrainingDbContext();
        
        string IdGenerator()
        {
            TrainingDbContext context = new TrainingDbContext();
            Course crs = context.Courses.OrderBy(x => x.CourseId).LastOrDefault();
            if(crs == null) { return "C-001"; }
            string[] idValue = crs.CourseId.Split('-');
            int number = Convert.ToInt32(idValue[1]);
            string newId = "C-" + (++number).ToString("d3");
            return newId;
        }
        //string CourseSchedule(string choice)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Console.Write("Enter Number of Days for Schedule:");
        //    string NoOfDays = StringInput();
        //    Validate val=new Validate();
        //    if(val.IsDigit(NoOfDays) && Convert.ToInt16(NoOfDays)<7)
        //    {
        //        string[] weekdays = new string[] { "SAT", "SUN", "MON", "TUE", "WED", "THU", "FRI" };
        //        string[] WEEKDAYS = new string[] { "SATURDAY", "SUNDAY", "MONDAY", "TUESDAY", "WEDNESDAY", "THURSDAY", "FRIDAY" };
        //        string[] courseDays = new string[Convert.ToInt16(NoOfDays)];
        //        Console.WriteLine("Please Enter Schedule in the spedcified format: Day Time: Sun 12:00 PM-02:00 PM");
        //        for (int i = 0; i < courseDays.Length; i++)
        //        {
        //            Console.Write("Insert week Day "+Convert.ToInt32((i+1))+":");
        //            string newDay = StringInput().ToUpper();
        //            for(int j = 0; j < weekdays.Length; j++)
        //            {
        //                if (newDay.ToUpper() ==weekdays[j] || newDay.ToUpper() == WEEKDAYS[j])
        //                {
                           
        //                    sb.Append(newDay+"  ");
        //                    Console.Write("Insert week Day " + Convert.ToInt32((i + 1)) +"Time :");
                            
        //                }
        //            }
        //            if(i< courseDays.Length-1)
        //            {
        //                sb.Append(", ");
        //            }

        //        }
        //        return sb.ToString();

        //    }
        //    else
        //    {
        //            return string.Empty;
        //    }
        //}
        string StringInput() //color coding for inputs
        {
            string input;
            Console.ForegroundColor = ConsoleColor.Green;
            input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            return input;
        }
        bool IsTeacherExist(string id)
        {
            TrainingDbContext context = new TrainingDbContext();
            Teacher teacher = context.Teachers.Where(x => x.TeacherId == id).FirstOrDefault();
            if(teacher == null) { return false; }
            if(teacher.TeacherId==id)
            {
                return true;
            }
            return false;
        }
        public void GetAllCourses()
        {
            //StringBuilder courseList = new StringBuilder();
            var table = new ConsoleTable ("Course Id", "Course Title", "Instructor", "Schedule", "Credit Fees"); 
           // courseList.Append("Course Id" + "\t\t" + "Course Title" + "\t\t" + "Instructor" + "\t\t" + "Schedule" + "\t\t" + "Credit Fees" + "\n");
            List<Course> courses = context.Courses.ToList();
            if (courses != null || courses.Count!=0) 
            {
                foreach (Course course in courses)
                {
                    table.AddRow(course.CourseId, course.CourseName, course.TeacherId, course.Schedule, course.Fees);
                    // courseList.Append(course.CourseId + "\t\t" + course.CourseName + "\t\t\t" + course.TeacherId + "\t\t" + course.Schedule + "\t\t" + course.Fees + "\n");
                }
            }
            table.Write();
        }
        public bool CourseCRUD(string key)
        {
            Console.Clear();
            GetAllCourses();
            List<Course> courses = context.Courses.ToList();
            if (key == "d")//insert new course
            {
                Course course = new Course();
                course.CourseId = IdGenerator();
                Console.WriteLine("Insert New Course..");
                Console.WriteLine("Course Id:" + course.CourseId + "  (Auto generated)");
                Console.Write("Course Title: ");
                course.CourseName = StringInput();
                Console.Write("Course Instructor: ");
                course.TeacherId = StringInput();
                Console.Write("Course Schedule: ");
                //course.Schedule = CourseSchedule("d");
                course.Schedule = StringInput();
                Console.Write("Course Fee: ");
                string Fee = StringInput();
                Validate validate = new Validate();
                if (!validate.IsValidName(course.CourseName) || !IsTeacherExist(course.TeacherId) ||!validate.IsDigit(Fee) || course.Schedule==string.Empty)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Teacher Id or Course Title..Press any key to Try Again");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press Escape to Cancel");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        return true;
                    }
                    CourseCRUD("d");
                }
                else
                {
                    course.Fees = Convert.ToInt32(Fee);
                    context.Courses.Add(course);
                    context.SaveChanges();
                    Console.WriteLine("Course Added Successfully..\n\nPress any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    return true;
                }
            }
            else if (key == "i")//update course info
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Update Course Info\n");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("1. Update Course\t2. Delete\t3.Cancel");
                Console.Write("Insert Choice: ");
                string choice = StringInput();
                try
                {
                    if (Int32.Parse(choice) == 1 || Int32.Parse(choice) == 2 || Int32.Parse(choice) == 3)
                    {
                        if (Int32.Parse(choice) == 1)
                        {
                            Console.Write("Insert Course Id: ");
                            string id = StringInput();
                            foreach (Course course in courses)
                            {
                                if (course.CourseId == id)
                                {
                                    Course course1 = context.Courses.Where(x => x.CourseId == id).FirstOrDefault();
                                    Console.WriteLine("Insert updated Info: ");
                                    Console.Write("Course Name: ");
                                    course1.CourseName = StringInput();
                                    Console.Write("TeacherId: ");
                                    course1.TeacherId = StringInput();
                                    Console.Write("Schedule: ");
                                    course1.Schedule = StringInput();
                                    Console.Write("Fees: ");
                                    course1.Fees = Convert.ToInt32(StringInput());
                                    Validate validate = new Validate();
                                    if (!validate.IsValidName(course.CourseName) || !IsTeacherExist(course.TeacherId) || !validate.IsDigit(course.Fees.ToString())) { break; }
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
                            Console.WriteLine("Invalid Info or Teacher doesn't exist.Press any key to Try Again");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Press Escape to Cancel");
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                Console.Clear();
                                return true;
                            }
                            CourseCRUD("i");
                        }                       
                        else if (Int32.Parse(choice) == 2)
                        {
                            Console.Write("Insert Course Id: ");
                            string id = StringInput();
                            foreach (Course course in courses)
                            {
                                if (course.CourseId == id)
                                {
                                    List<Enroll> enroll = context.Enrolls.Where(x => x.CourseId == id).ToList();
                                    foreach(Enroll enroll_ in enroll)
                                    {
                                        context.Remove(enroll_);
                                        context.SaveChanges();
                                    }
                                    List<Attendance> atn = context.Attendance.Where(x => x.CourseId == id).ToList();
                                    foreach (Attendance attendance in atn)
                                    {
                                        context.Remove(attendance);
                                        context.SaveChanges();
                                    }
                                    Course crs = context.Courses.Where(x => x.CourseId == id).FirstOrDefault();
                                    context.Courses.Remove(crs);
                                    context.SaveChanges();
                                    Console.WriteLine("Course Deleted Successfully..\n\nPress any key to continue");
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
                            CourseCRUD("i");
                        }
                        else if (Int32.Parse(choice) == 3)
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
                        CourseCRUD("i");
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
                    CourseCRUD("i");
                }
            }
            return true;
        }
    }
}
