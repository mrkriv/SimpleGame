using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OneCGame.Server
{
    public class Token
    {
        public Models.User User { get; set; }
        public DateTime AuthDateTime { get; set; }
    }
}
