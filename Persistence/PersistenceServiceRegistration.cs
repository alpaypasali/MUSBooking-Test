﻿using Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence;

    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
        

            services.AddDbContext<BaseDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("BaseDbContext")));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<IOrderEquipmentRepository, OrderEquipmentRepository>();
       



            return services;
        }
    }


