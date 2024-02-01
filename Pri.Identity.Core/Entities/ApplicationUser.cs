using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pri.Identity.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string City { get; set; }
    }
}
