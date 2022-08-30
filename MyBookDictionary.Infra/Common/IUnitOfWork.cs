using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Common
{
    public interface IUnitOfWork
    {
        void Dispose();
        Task SaveChanges();
    }
}
