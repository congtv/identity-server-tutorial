using IdentityServerTutorial.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServerTutorial.Models
{
    public class ResultModel
    {
        public Status Status { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
