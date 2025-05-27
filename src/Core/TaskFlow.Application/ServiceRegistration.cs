using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskFlow.Application.Mappings;
namespace TaskFlow.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddAutoMapper(typeof(ProjectProfile));


            // CQRS Handler'ları otomatik tanıması için
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });


            
            return services;
        }
    }
}
