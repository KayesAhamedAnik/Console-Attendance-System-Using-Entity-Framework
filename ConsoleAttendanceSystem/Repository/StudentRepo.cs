using ConsoleAttendanceSystem.Entities;
using ConsoleAttendanceSystem.EntityFramework;
using ConsoleAttendanceSystem.Validation;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Repository
{
    public class StudentRepo
    {
        TrainingDbContext context = new TrainingDbContext();
        string IdGenerator()
        {
            
            Student st = context.Students.OrderBy(x => x.StudentId).LastOrDefault();
            if (st == null) { return "S-001"; }
            string[] idValue = st.StudentId.Split('-');
            int number = Convert.ToInt32(idValue[1]);
            string newId = "S-" + (++number).ToString("d3");
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
        public void GetAllStudents()
        {
            //StringBuilder studentList = new StringBuilder();
            //studentList.Append("Student Id" + "\t" + "Name\n");
            var table = new ConsoleTable("Student Id", "Name");
            List<Student> Students = context.Students.ToList();
            if(Students.Count!=0 || Students != null)
            {
                foreach (Student students in Students)
                {
                    //studentList.Append(students.StudentId + "\t\t" + students.Name + "\n");
                    table.AddRow(students.StudentId, students.Name);
                }
            }
            table.Write();
            //Console.WriteLine(studentList);
        }
        public bool StudentCRUD(string key)
        {
            Console.Clear();
            GetAllStudents();
            List<Student> Students = context.Students.ToList();
            if (key == "b")//insert new admin
            {
                Student student = new Student();
                student.StudentId = IdGenerator();
                Console.WriteLine("Insert New Student.");
                Console.WriteLine("Student Id:" + student.StudentId+ "  (Auto generated)");
                    Console.Write("Name: ");
                    student.Name = StringInput();
                    Console.Write("Password: ");
                    student.Password = StringInput();
                    Validate validate = new Validate();
                    if (!validate.IsValidName(student.Name) || !validate.IsPasswordValid(student.Password))
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
                        StudentCRUD("b");
                    }
                    else
                    {
                        context.Students.Add(student);
                        context.SaveChanges();
                        Console.WriteLine("Student Added Successfully..\n\nPress any key to continue");
                        Console.ReadKey();
                        Console.Clear();
                        return true;
                       
                    }
            }
            else if (key == "g")//update name or pass
            {

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Update Student\n");
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
                            Console.Write("Insert Student Id: ");
                            string id = StringInput();
                            foreach (Student student in Students)
                            {
                                if (student.StudentId == id)
                                {
                                    Student student1 = context.Students.Where(x => x.StudentId == id).FirstOrDefault();
                                    Console.Write("Insert updated Name: ");
                                    student1.Name = StringInput();
                                    Validate validate = new Validate();
                                    if (!validate.IsValidName(student1.Name)) { break; }
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
                            Console.WriteLine("Invalid Name or Id..Press any key to Try Again");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Press Escape to Cancel");
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                Console.Clear();
                                return true;
                            }
                            StudentCRUD("g");


                        }
                        else if (Int32.Parse(choice) == 2)
                        {
                            Console.Write("Insert Student Id: ");
                            string id = StringInput();
                            foreach (Student student in Students)
                            {
                                if (student.StudentId == id)
                                {
                                    Student student1 = context.Students.Where(x => x.StudentId == id).FirstOrDefault();
                                    Console.Write("Insert updated Password: ");
                                    student1.Password = StringInput();
                                    Validate validate = new Validate();
                                    if (!validate.IsPasswordValid(student1.Password)) { break; }
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
                            Console.WriteLine("Invalid Id or Password..Press any key to Try Again");
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Press Escape to Cancel");
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                Console.Clear();
                                return true;
                            }
                            StudentCRUD("g");
                        }
                        else if (Int32.Parse(choice) == 3)
                        {
                            Console.Write("Insert Admin Id: ");
                            string id = StringInput();
                            foreach (Student student in Students)
                            {
                                if (student.StudentId == id)
                                {
                                    List<Enroll> enroll = context.Enrolls.Where(x => x.CourseId == id).ToList();
                                    foreach (Enroll enroll_ in enroll)
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
                                    Student st = context.Students.Where(x => x.StudentId == id).FirstOrDefault();
                                    context.Students.Remove(st);
                                    context.SaveChanges();
                                    Console.WriteLine("Student Deleted Successfully..\n\nPress any key to continue");
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
                            StudentCRUD("g");
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
                        StudentCRUD("g");
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
        void AllCourses(string id)
        {
            EnrollRepo en=new EnrollRepo();
            var table = new ConsoleTable("Enroll Id", "Course Id", "Course Title", "Teacher Id", "Teacher Name", "Student Id");
            var enrolls = context.Enrolls.Where(x=>x.StudentId==id).ToList();
            if (enrolls != null || enrolls.Count != 0)
            {
                foreach (Enroll enroll in enrolls)
                {
                    table.AddRow(enroll.EnrollId, enroll.CourseId, en.GetCourseTitle(enroll.CourseId), enroll.TeacherId, en.GetTeachersName(enroll.TeacherId), enroll.StudentId);
                    // studentList.Append(enroll.EnrollId + "\t\t" + enroll.CourseId + "\t\t" + GetCourseTitle(enroll.CourseId) + "\t\t" + enroll.TeacherId + "\t\t" + GetTeachersName(enroll.TeacherId) + "\t\t" + enroll.StudentId + "\n");
                }
            }
            // Console.WriteLine(studentList);
            table.Write();
        }
        bool IsCourseIdValid(string sid, string cid)
        {
            Enroll enroll = context.Enrolls.Where(x => x.StudentId == sid && x.CourseId == cid).FirstOrDefault();
            if (enroll != null) { return true; }
            return false;
        }
        public void StudentMenu(string id)
        {
            //Console.Clear();
            Console.WriteLine("Logged in as Student\t\t\tStudent Id:"+id);
            Console.WriteLine("\nEnrolled Courses:");
            AllCourses(id);
            Console.WriteLine("1. Give Attendence 2. Check Attendance");
            Console.Write("Insert Choice:");
            string choice=StringInput();
            Validate val=new Validate();
            if(val.IsDigit(choice))
            {

                if(Convert.ToInt32(choice)==1 || Convert.ToInt32(choice) == 2)
                {
                    Console.Write("Insert Course Id: ");
                    string cid = StringInput();
                    if(IsCourseIdValid(id,cid))
                    {
                        if (Convert.ToInt32(choice) == 1) //give attendance
                        {
                            AttendanceRepo attendanceRepo = new AttendanceRepo();
                            if(attendanceRepo.InsertAttendance(cid,id))
                            {
                                    Console.WriteLine("Attendance Updated");
                                    System.Threading.Thread.Sleep(1000);
                                    Console.Clear();
                                    StudentMenu(id);
                            }
                            else
                            {
                                Console.WriteLine("Attendance Already Inserted");
                                Console.ReadKey();
                                Console.Clear();
                                StudentMenu(id);
                            }

                        }
                        else if (Convert.ToInt32(choice) == 2) //check attendance
                        {
                            AttendanceRepo attendanceRepo = new AttendanceRepo();
                            attendanceRepo.viewStudentAttendance(cid,id);
                            Console.WriteLine("Press any key to go back");
                            Console.WriteLine("Press Escape to Logout");
                            if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                            {
                                Environment.Exit(0);
                            } 
                            Console.Clear();
                            StudentMenu(id);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");
                        Console.ReadKey();
                        Console.Clear();
                        StudentMenu(id);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid Input.Press any key to continue");
                Console.ReadKey();
                Console.Clear();
                StudentMenu(id);
            }
        }

    }
}
