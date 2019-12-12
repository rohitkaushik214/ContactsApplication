using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ContactsApplication.Domain;

namespace ContactsApplication.Repository
{
    public class GetContactsResponse
    {
        public bool hasMore { get; set; }

        public List<Contact> Contacts { get; set; }

        public int offset { get; set; }
    }
}