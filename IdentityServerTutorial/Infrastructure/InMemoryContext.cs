using IdentityServerTutorial.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerTutorial.Infrastructure
{
    public static class InMemoryContext
    {
        static InMemoryContext()
        {
            Users = new List<ApplicationUser>();
        }
        public static List<ApplicationUser> Users { get;}
    }
}
