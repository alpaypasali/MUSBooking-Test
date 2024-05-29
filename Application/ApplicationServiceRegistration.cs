﻿using Application.Features.Equipments.Commands.Create;
using Application.Features.Equipments.Commands.Update;
using Application.Features.Equipments.Rules;
using Application.Features.Orders.Rules;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());


        });

        services.AddValidatorsFromAssemblyContaining<CreatedEquipmentCommandValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdatedEquipmentCommandValidator>();
        services.AddScoped<EquipmentBusinessRules>();
        services.AddScoped<OrderBusinessRules>();

        return services;


    }
    public static IServiceCollection AddSubClassesOfType(
      this IServiceCollection services,
      Assembly assembly,
      Type type,
      Func<IServiceCollection, Type, IServiceCollection>? addWithLifeCycle = null
   )
    {
        var types = assembly.GetTypes().Where(t => t.IsSubclassOf(type) && type != t).ToList();
        foreach (var item in types)
            if (addWithLifeCycle == null)
                services.AddScoped(item);

            else
                addWithLifeCycle(services, type);
        return services;
    }
}