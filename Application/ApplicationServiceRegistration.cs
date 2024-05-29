
using Application.Services.EquipmentServices;
using FluentValidation;
using Infrastructure.Behavior.Validation;
using Infrastructure.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                configuration.AddOpenBehavior(typeof(RequestValidationBehavior<,>));
            });

       
            services.AddSubClassesOfType(Assembly.GetExecutingAssembly(), typeof(BaseBusinessRules));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddScoped<IEquipmentService, EquipmentManager>();
            return services;
        }

        public static IServiceCollection AddSubClassesOfType(
            this IServiceCollection services,
            Assembly assembly,
            Type baseType,
            Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
        )
        {
            var types = assembly.GetTypes().Where(t => t.IsSubclassOf(baseType) && baseType != t).ToList();
            foreach (var type in types)
            {
                if (addWithLifeCycle == null)
                {
                    services.AddScoped(type);
                }
                else
                {
                    addWithLifeCycle(services, type);
                }
            }
            return services;
        }
    }
}
