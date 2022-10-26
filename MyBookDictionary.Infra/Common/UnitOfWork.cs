using MyBookDictionary.Infra.Context.Identity;
using MyBookDictionary.Infra.Context.Main;
using MyBookDictionary.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Common
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MainContext _mainContext;
        private readonly IdentityContext _identityContext;
        public IContextClassService contextClassService { get; }

        public UnitOfWork(MainContext mainContext, IdentityContext identityContext, IContextClassService contextClassService)
        {
            _mainContext = mainContext;
            _identityContext = identityContext;
            this.contextClassService = contextClassService;
        }
        private bool disposed = false;

        public void Dispose()
        {
            if(!disposed)
            {
                _mainContext.Dispose();
                _identityContext.Dispose();
            }
            disposed = true;
        }

        public async Task SaveChanges()
        {
            await _mainContext.SaveChangesAsync(CancellationToken.None);
            await _identityContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
