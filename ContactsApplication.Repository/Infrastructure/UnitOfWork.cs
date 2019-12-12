using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsApplication.Domain;

namespace ContactsApplication.Repository
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IDatabaseFactory databaseFactory;
        private ContactsApplicationDBEntities dataContext;

        public UnitOfWork(IDatabaseFactory databaseFactory)
        {
            this.databaseFactory = databaseFactory;
        }

        public ContactsApplicationDBEntities DataContext
        {
            get { return dataContext ?? (dataContext = databaseFactory.Get()); }
        }

        public void Commit()
        {
            DataContext.SaveChanges();
        }
    }
}
