using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Papuce
{
    public enum UserType
    {
        Admin,
        Guest
    }
    public class User
    {

        public string Username { get; set; }
        public string Password { get; set; }
        public UserType Type { get; set; }

        public User(string username, string password, UserType type)
        {
            Username = username;
            Password = password;
            Type = type;
        }
    }
}
