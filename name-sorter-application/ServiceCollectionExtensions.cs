using Microsoft.Extensions.DependencyInjection;
using name_sorter.application.BusinessLogic.Helper;
using name_sorter.application.BusinessLogic.Services;
using name_sorter.application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace name_sorter.application
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the services required for the name sorter application to the service collection.
        /// </summary>
        /// <param name="services">The service collection to add services to.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddNameSorterApplicationServices(this IServiceCollection services)
        {
            // Register application services
            services.AddScoped<IPersonService, PersonService>();
            services.AddScoped<IPersonParser, PersonParser>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IPersonSorter, PersonSorter>();
            return services;
        }
    }
}
