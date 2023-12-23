using ConsoleAttendanceSystem.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAttendanceSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConsoleAttendanceSystem.Repository
{
    public class LoginRepo
    {
        string UserName { get; set; }
        string Password { get; set; }
        public LoginRepo(string userName, string Password)
        {
            this.UserName = userName;
            this.Password = Password;
            //Login();
        }
        public int Login()
        {
            bool result=true;
            TrainingDbContext context = new TrainingDbContext();
            if (UserName[0] == 'A')
            {
                Admin c1 = context.Admins.Where(x => x.AdminId == UserName).FirstOrDefault();
                if(c1 == null) { return 0; }
                else
                {
                    if (c1.Password == Password && c1.AdminId == UserName)
                    {
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if(UserName[0] == 'T')
            {
                Teacher c1 = context.Teachers.Where(x => x.TeacherId == UserName).FirstOrDefault();
                if (c1 == null) { return 0; }
                else
                {
                    if (c1.Password == Password && c1.TeacherId == UserName)
                    {
                        return 2;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            else if(UserName[0] == 'S')
            {
                Student c1 = context.Students.Where(x => x.StudentId == UserName).FirstOrDefault();
                if (c1 == null) { return 0; }
                else
                {
                    if (c1.Password == Password && c1.StudentId == UserName)
                    {
                        return 3;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            return 0;
        }

    }
}
