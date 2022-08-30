using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyBookDictionary.Model.Common.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Context.Main
{
    public class MainContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public MainContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("BookClient"));
            base.OnConfiguring(optionsBuilder);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdateDate = DateTime.Now;
                }
                else if(entry.State == EntityState.Deleted)
                {
                    entry.Entity.DeletedDate = DateTime.Now;
                }
            }

            return await base.SaveChangesAsync(CancellationToken.None);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
