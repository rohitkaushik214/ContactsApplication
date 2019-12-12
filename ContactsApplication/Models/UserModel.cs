using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsApplication
{
    public class UserModel : IdentityUser
    {
        public string UserName { get; set; }
        
        public string Password { get; set; }

        public string Name { get; set; }

    }
}