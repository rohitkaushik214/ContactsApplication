using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContactsApplication.Domain;

namespace ContactsApplication.Repository
{
    public interface IDatabaseFactory: IDisposable
    {
        ContactsApplicationDBEntities Get();
    }
}
