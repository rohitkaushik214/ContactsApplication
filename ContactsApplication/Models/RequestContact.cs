using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactsApplication
{
    public class RequestContact
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
    }
}