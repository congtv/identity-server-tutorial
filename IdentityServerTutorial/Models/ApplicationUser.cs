using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Threading.Tasks;

namespace IdentityServerTutorial.Models
{
    [Serializable]
    public class ApplicationUser
    {
        public string ID { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string NormalizeUserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
