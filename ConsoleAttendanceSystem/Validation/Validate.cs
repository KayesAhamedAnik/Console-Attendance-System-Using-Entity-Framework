using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAttendanceSystem.Validation
{
    public class Validate
    {
        public bool IsValidName(string name)
        {
            if (name == null || name.Length<4)
            {
                return false;
            }
            else if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                return false;
            }
            return true;
        }
        public bool IsDigit(string digit)
        {
            int number = 0;
            if (int.TryParse(digit, out number))
            {
                return true;
            }
            return false;
        }
        public bool IsPasswordValid(string pass)
        {
            if(pass == null || pass.Length<4)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
