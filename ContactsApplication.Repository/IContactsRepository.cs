using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContactsApplication.Domain;

namespace ContactsApplication.Repository
{
    public interface IContactsRepository
    {
        Contact GetContactById(int contactId);

        GetContactsResponse GetContacts(int offset);

        void AddContact(Contact contact);

        void UpdateContact(Contact contact);
    }
}
