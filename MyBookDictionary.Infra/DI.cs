using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyBookDictionary.Infra.Context.Identity;
using MyBookDictionary.Infra.Context.Main;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddDbContext<IdentityContext>(opt => opt.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=BookDict;Integrated Security=True;"));
            services.AddDbContext<MainContext>(opt => opt.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=BookDict;Integrated Security=True;"));

            return services;
        }
    }
}
