using ContactsApplication.Domain;

namespace ContactsApplication.Repository
{
    public class DatabaseFactory: IDatabaseFactory
    {
        private ContactsApplicationDBEntities _dataContext;
        public ContactsApplicationDBEntities Get()
        {
            if (_dataContext == null)
            {
                _dataContext = new ContactsApplicationDBEntities();
            }

            return _dataContext;
        }

        public void Dispose()
        {
            if (_dataContext != null)
            {
                _dataContext.Dispose();
            }
        }
    }
}
