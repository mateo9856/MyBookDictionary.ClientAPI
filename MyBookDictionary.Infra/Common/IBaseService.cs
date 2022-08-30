using MyBookDictionary.Model.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Common
{
    public interface IBaseService<T> where T : BaseEntity
    {
        Task<T> GetItem(Guid id);
        Task<IEnumerable<T>> SelectAll();
        Task Add(T item);
        Task<T> Update(T item);
        void Delete(T item);
    }
}
