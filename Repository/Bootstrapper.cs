using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository
{
    public class Bootstrapper
    {
        public static void Initialize(IServiceCollection services, 
            DbContextOptionsBuilder<ProductContext> options)
        {
            services.AddTransient<ProductContext>(x => new ProductContext(options.Options));
            services.AddTransient<IInventoryRepository, InventoryRepository>();
        }
    }
}
