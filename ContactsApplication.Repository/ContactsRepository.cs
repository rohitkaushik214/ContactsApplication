using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsApplication.Domain;
using log4net;

namespace ContactsApplication.Repository
{
    public class ContactsRepository: RepositoryBase<Contact>, IContactsRepository
    {
        private const int MAX_NO_OF_CONTACTS = 200;
        private IDatabaseFactory dbFactory;
        private ILog log;

        public ContactsRepository(IDatabaseFactory databaseFactory, ILog log)
            : base(databaseFactory)
        {
            this.dbFactory = databaseFactory;
            this.log = log;
        }
        
        public Contact GetContactById(int contactId)
        {
            return DataContext.Contacts.Where(x => x.Id == contactId).FirstOrDefault();
        }

        public GetContactsResponse GetContacts(int offset)
        {
            var response = new GetContactsResponse();

            var contactCount = DataContext.Contacts.Count();
            try
            {
                if (contactCount > offset)
                {
                    response.Contacts = DataContext.Contacts.OrderBy(x=>x.Id).Skip(offset).Take(MAX_NO_OF_CONTACTS).ToList();
                    if (contactCount > (response.Contacts.Count + offset))
                    {
                        response.hasMore = true;
                        response.offset = response.Contacts.Count;
                    }

                    return response;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error occured in GetContacts repository:", ex);
            }

            response.offset = 0;
            response.Contacts = new List<Contact>();
            return response;
        }

        public void AddContact(Contact contact)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            this.Add(contact);
            unitOfWork.Commit();
        }

        public void UpdateContact(Contact contact)
        {
            var unitOfWork = new UnitOfWork(dbFactory);
            this.Update(contact);
            unitOfWork.Commit();
        }
    }
}
