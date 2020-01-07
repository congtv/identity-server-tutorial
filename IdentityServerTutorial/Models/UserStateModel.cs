using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerTutorial.Models
{
    public class UserStateModel
    {
        public bool IsAuthenticated { get; set; }
        public string Username { get; set; }
    }
}
