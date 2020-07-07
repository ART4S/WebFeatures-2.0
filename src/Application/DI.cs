﻿using Application.Behaviours;
using Application.Extensions;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Application
{
    public static class DI
    {
        public static void AddApplication(this IServiceCollection services)
        {
            AddPipeline(services);
            AddMapperProfiles(services);
            AddValidators(services);
        }

        private static void AddPipeline(IServiceCollection services)
        {
            services.AddMediatR(typeof(DI).Assembly);

            // Common
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));

            // Commands
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(SaveChangesBehaviour<,>));
        }

        private static void AddMapperProfiles(IServiceCollection services)
        {
            var configuration = new MapperConfiguration(config =>
            {
                Type[] profileTypes = typeof(DI).Assembly
                    .GetTypes()
                    .Where(x => x.IsSubclassOf(typeof(Profile)))
                    .ToArray();

                foreach (Type profileType in profileTypes)
                {
                    config.AddProfile(profileType);
                }
            });

            configuration.AssertConfigurationIsValid();

            services.AddSingleton(configuration.CreateMapper());
        }

        private static void AddValidators(IServiceCollection services)
        {
            var validators = typeof(DI).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidator<>)))
                .Select(x => new
                {
                    Service = x.GetInterfaces().First(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IValidator<>)),
                    Implementation = x
                })
                .ToArray();

            foreach (var validator in validators)
            {
                services.AddScoped(validator.Service, validator.Implementation);
            }
        }
    }
}