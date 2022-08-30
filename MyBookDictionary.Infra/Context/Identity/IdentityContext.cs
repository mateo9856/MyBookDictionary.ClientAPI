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
        private readonly IConfiguration _configuration;
        public IdentityContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbSet<AccountUser> Users { get; set; }

        public DbSet<UserRole> UsersRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("BookClient"));
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
