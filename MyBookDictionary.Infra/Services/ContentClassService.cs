using Microsoft.EntityFrameworkCore;
using MyBookDictionary.Infra.Context.Main;
using MyBookDictionary.Infra.Interfaces;
using MyBookDictionary.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Services
{
    public class ContentClassService : BaseService<ContentClasses>, IContextClassService
    {
        private readonly MainContext mainContext;

        public ContentClassService(MainContext context) : base(context)
        {
            mainContext = context;
        }

        public async Task<ContentClasses[]> AllParams()
        {
            return await mainContext.ContentClasses.ToArrayAsync();
        }

        public async Task<ContentClasses[]> GetFromParam(params string[] contents)
        {
            return await mainContext.ContentClasses
                .Where(c => contents.Contains(c.ContentClassName))
                .ToArrayAsync();
        }

    }
}
