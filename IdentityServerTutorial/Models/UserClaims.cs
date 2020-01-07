using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerTutorial.Models
{
    public class UserClaims
    {
        public IEnumerable<ClaimResultModel> Claims { get; set; }
        public string UserName { get; set; }
    }
}
