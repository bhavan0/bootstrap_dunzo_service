using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Bootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddTransient<IImageService, ImageService>();  
        }
    }
}
