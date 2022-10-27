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

        public async Task<string[]> AllParams()
        {
            return await mainContext.ContentClasses.Select(c => c.ContentClassName).ToArrayAsync();
        }

        public async Task<string[]> GetFromParam(params string[] contents)
        {
            return await mainContext.ContentClasses
                .Where(c => contents.Any(d => d.Contains(c.ContentClassName)))
                .Select(x => x.ContentClassName).ToArrayAsync();
        }

    }
}
