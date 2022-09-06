using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyBookDictionary.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Context.Identity
{
    public class IdentityContext : DbContext
    {
        public IdentityContext(DbContextOptions<IdentityContext> options) : base(options)
        {
        }

        public DbSet<AccountUser> Users { get; set; }

        public DbSet<UserRole> UsersRoles { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<AccountUser>())
            {
                if (entry.State == EntityState.Deleted)
                {
                    entry.Entity.DeleteDate = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("identity");

            modelBuilder.Entity<UserRole>()
                .HasKey(c => c.RoleId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
