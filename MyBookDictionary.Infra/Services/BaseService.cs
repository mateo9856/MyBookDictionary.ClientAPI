using Microsoft.EntityFrameworkCore;
using MyBookDictionary.Infra.Common;
using MyBookDictionary.Infra.Context.Main;
using MyBookDictionary.Model.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Services
{
    public class BaseService<T> : IBaseService<T> where T : BaseEntity
    {
        private MainContext _context;
        private DbSet<T> _entity;
        public BaseService(MainContext context)
        {
            _context = context;
            _entity = context.Set<T>();
        }

        public async Task Add(T item)
        {
            await _entity.AddAsync(item);
        }

        public void Delete(T item)
        {
             _entity.Remove(item);
        }

        public async Task<T> GetItem(Guid id)
        {
            return await _entity.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<IEnumerable<T>> SelectAll()
        {
            return await _entity.ToListAsync();
        }

        public async Task<T> Update(T item)
        {
            _entity.Attach(item);
            return await GetItem(item.Id);
        }
    }
}
