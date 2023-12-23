using ConsoleAttendanceSystem.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

public class Program
{
     static void Main(string[] args)
    {

        Input();
    }
     static string StringInput() //color coding for inputs
    {
        string input;
        Console.ForegroundColor = ConsoleColor.Green;
        input =Console.ReadLine();
        Console.ForegroundColor = ConsoleColor.White;
        return input;
    }
    public static void Input()
    {
        string userType = "LogIn";
        string s = "Welcome" + "\n" + "*** Student Attendance and Course Management System ***" + "\n\n\n" + userType;
        Console.SetCursorPosition((Console.WindowWidth - s.Length) / 2, Console.CursorTop);
        Console.WriteLine(s);

            Console.Write("User Id: ");
            string id = StringInput();
            Console.Write("Password: ");
            string pass = StringInput();
        if(id!=string.Empty && pass!=string.Empty)
        {
            LoginRepo loginRepo = new LoginRepo(id, pass);
            int result = loginRepo.Login();
            if (result == 0)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Credentials.\n Please try again.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Press any key to Continue...");
                Console.ReadKey();
                Console.Clear();
                Input();
            }
            else if (result == 1)
            {
                AdminRepo adminRepo = new AdminRepo();
            }
            else if (result == 2)
            {
                TeacherRepo teacherRepo = new TeacherRepo();
                teacherRepo.TeachersMenu(id);
            }
            else if (result == 3)
            {
                StudentRepo student = new StudentRepo();
                Console.Clear();
                student.StudentMenu(id);
            }
            else
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Input\tPlease try again.");
                Console.ForegroundColor = ConsoleColor.White;
                Input();
            }
        }
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid Credentials.\n Please try again.");
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Press any key to Continue...");
        Console.ReadKey();
        Console.Clear();
        Input();

    }
}

