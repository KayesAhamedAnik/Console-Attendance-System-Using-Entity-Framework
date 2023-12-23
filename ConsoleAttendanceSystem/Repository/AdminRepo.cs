using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAttendanceSystem.EntityFramework;
using ConsoleAttendanceSystem.Entities;
using ConsoleAttendanceSystem.Validation;
using ConsoleTables;
namespace ConsoleAttendanceSystem.Repository
{
    internal class AdminRepo
    {
        string key = null;
        public AdminRepo()
        {
            Console.Clear();
            SelectionMenu();
        }

        void SelectionMenu()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("Welcome to Admin Panel\n\n");
            stringBuilder.Append("Please type corresponding letters A through D to perform task.\n\n");
            stringBuilder.Append("1. Create New\n");
            stringBuilder.Append("\tA. Admin\n");
            stringBuilder.Append("\tB. Student\n");
            stringBuilder.Append("\tC. Teacher\n");
            stringBuilder.Append("\tD. Course\n");
            stringBuilder.Append("2. Enroll\n");
            stringBuilder.Append("\tE. Assign Student\n");
            stringBuilder.Append("3. Update or Delete\n");
            stringBuilder.Append("\tF. Admin\n");
            stringBuilder.Append("\tG. Student\n");
            stringBuilder.Append("\tH. Teacher\n");
            stringBuilder.Append("\tI. Course\n");
            stringBuilder.Append("\tJ. Delete Enrolls\n");
            stringBuilder.Append("X. Logout\n"); 
            Console.WriteLine(stringBuilder);
            Console.Write("Insert Choice: ");
            key = StringInput().ToLower();
            if(key=="a" || key=="b"|| key == "c" || key == "d" || key == "e" || key == "f" || key == "g" || key == "h" || key=="i" || key == "j")
            {
                AdminOperations(key);
            }
            else if(key == "x")
            {
                Environment.Exit(0);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Choice\n  Please try Again\n\n");
                Console.ForegroundColor = ConsoleColor.White; 
                SelectionMenu();
            }
        }
        string StringInput() //color coding for inputs
        {
            string input;
            Console.ForegroundColor = ConsoleColor.Green;
            input = Console.ReadLine();
            Console.ForegroundColor = ConsoleColor.White;
            return input;
        }
        void AdminOperations(string key)
        {
            Console.Clear();
            //Console.WriteLine(IdGenerator());
            if(key== "a" || key == "f")// admin crud operations
            {
               AdminCRUD(key);
            }
            else if(key == "b" || key == "g")
            {
                StudentRepo studentRepo=new StudentRepo();
                bool result=studentRepo.StudentCRUD(key);
                if (result)
                {
                    SelectionMenu();
                }
                else
                {
                    AdminOperations(key);
                }
            }
            else if (key == "c" || key == "h")
            {
                TeacherRepo teacherRepo = new TeacherRepo();
                bool result = teacherRepo.TeacherCRUD(key);
                if (result)
                {
                    SelectionMenu();
                }
                else
                {
                    AdminOperations(key);
                }
            }
            else if (key == "d" || key == "i")
            {
                CourseRepo courseRepo = new CourseRepo();
                bool result = courseRepo.CourseCRUD(key);
                if (result)
                {
                    SelectionMenu();
                }
                else
                {
                    AdminOperations(key);
                }
            }
            else if (key == "e" || key == "j")
            {
                EnrollRepo enrollRepo = new EnrollRepo();
                bool result = enrollRepo.EnrollCRUD(key);
                if (result)
                {
                    SelectionMenu();
                }
                else
                {
                    AdminOperations(key);
                }
            }
        }
        void AdminCRUD(string key)
        {
            //StringBuilder adminlist = new StringBuilder();
            //adminlist.Append("Admin Id"+"\t"+"Name\n");
            var table = new ConsoleTable("Admin Id", "Name");
            Console.Clear();
            TrainingDbContext context = new TrainingDbContext();
            List<Admin> Admins = context.Admins.ToList(); 
            foreach (Admin admin in Admins)
            {
                //adminlist.Append(admin.AdminId+"\t\t"+admin.Name+"\n");
                table.AddRow(admin.AdminId, admin.Name);
            }
            //Console.WriteLine(adminlist);
            table.Write();
            if(key=="a")//insert new admin
            {
                Admin admin= new Admin();
                admin.AdminId = IdGenerator();
                Console.WriteLine("Insert New Admin..");
                Console.WriteLine("Admin Id:"+admin.AdminId+"  (Auto generated)");
                Console.Write("Name: ");
                admin.Name=StringInput();
                Console.Write("Password: ");
                admin.Password= StringInput();
                Validate validate = new Validate();
                if (!validate.IsValidName(admin.Name) || !validate.IsPasswordValid(admin.Password))
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid Name or Pass..Press any key to Try Again");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press Escape to Cancel");
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        SelectionMenu();
                    }
                    AdminCRUD("a");


                }
                else
                {
                    context.Admins.Add(admin);
                    context.SaveChanges();
                    Console.WriteLine("Admin Added Successfully..\n\nPress any key to continue");
                    Console.ReadKey();
                    Console.Clear();
                    SelectionMenu();
                }
                
            }
            else if(key=="f")//update name or pass
            { 
                
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Update Admin\n");
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
                            Console.Write("Insert Admin Id: ");
                            string id = StringInput();
                            foreach (Admin admin in Admins)
                            {
                                if (admin.AdminId == id)
                                {
                                    Admin admin2 = context.Admins.Where(x => x.AdminId == id).FirstOrDefault();
                                    Console.Write("Insert updated Name: ");
                                    admin2.Name = StringInput();
                                    Validate validate=new Validate();
                                    if (!validate.IsValidName(admin2.Name)) { break; }
                                    else
                                    {
                                        context.SaveChanges();
                                        Console.WriteLine("Updated Successfully..\n\nPress any key to continue");
                                        Console.ReadKey();
                                        Console.Clear();
                                        SelectionMenu();
                                        break;
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
                                SelectionMenu();
                            }
                            AdminCRUD("f");
                        }
                        else if (Int32.Parse(choice) == 2)
                        {
                            Console.Write("Insert Admin Id: ");
                            string id = StringInput();
                            foreach (Admin admin in Admins)
                            {
                                if (admin.AdminId == id)
                                {
                                    Admin admin2 = context.Admins.Where(x => x.AdminId == id).FirstOrDefault();
                                    Console.Write("Insert updated Password: ");
                                    admin2.Password = StringInput();
                                    //context.Admins.Update(admin2);
                                    Validate validate = new Validate();
                                    if (!validate.IsPasswordValid(admin2.Password)) { break; }
                                    else
                                    {
                                        context.SaveChanges();
                                        Console.WriteLine("Updated Successfully..\n\nPress any key to continue");
                                        Console.ReadKey();
                                        Console.Clear();
                                        SelectionMenu();
                                        break;
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
                                SelectionMenu();
                            }
                            AdminCRUD("f");
                        }
                        else if (Int32.Parse(choice) == 3)
                        {
                            Console.Write("Insert Admin Id: ");
                            string id = StringInput();
                            foreach (Admin admin in Admins)
                            {
                                if (admin.AdminId == id)
                                {
                                    Admin ad = context.Admins.Where(x => x.AdminId == id).FirstOrDefault();
                                    context.Admins.Remove(ad);
                                    if (ad.AdminId == "A-001") 
                                    {
                                        Console.WriteLine("Cannot Delete Primary Admin");
                                        SelectionMenu();
                                    }
                                    context.SaveChanges();
                                    Console.WriteLine("Deleted Successfully..\n\nPress any key to continue");
                                    Console.ReadKey();
                                    Console.Clear();
                                    SelectionMenu();
                                    break;
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
                                SelectionMenu();
                            }
                            AdminCRUD("f");
                        }
                        else if (Int32.Parse(choice) == 4)
                        {
                            Console.Clear();
                            SelectionMenu();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Choice..Press any key to Try Again");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        AdminCRUD("f");
                    }
                }
                catch
                {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid Choice..Press any key to Try Again");
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.ReadKey();
                        AdminCRUD("f");
                }
            }
        }
        string IdGenerator()
        {
            TrainingDbContext context = new TrainingDbContext();
            Admin ad=context.Admins.OrderBy(x => x.AdminId).LastOrDefault();
            string[] idValue =ad.AdminId.Split('-');
            int number = Convert.ToInt32(idValue[1]);
            string newId ="A-"+(++number).ToString("d3");
            return newId;
        }
   
    }
}
