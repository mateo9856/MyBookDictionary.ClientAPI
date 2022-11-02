using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyBookDictionary.Infra.Context.Identity;
using MyBookDictionary.Model.Common.Base;
using MyBookDictionary.Model.Entities;
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

        public MainContext()
        {

        }

        public MainContext(DbContextOptions<MainContext> options) : base(options)
        {
        }

        public DbSet<ContentClasses> ContentClasses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=BookDict;Integrated Security=True;");
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
