using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Databases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastruture(this IServiceCollection services, string connectionString)
        {
            services.AddSingleton<FakeDatabase>();
            services.AddDbContext<RealDatabase>(options =>
            {
                options.UseSqlServer(connectionString);
        });
            return services;
        }
    }
}
