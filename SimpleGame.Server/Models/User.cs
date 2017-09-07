using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCGame.Server.Models
{
    public class User : IEquatable<User>
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public bool Equals(User other)
        {
            return other?.ID == ID;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }
}