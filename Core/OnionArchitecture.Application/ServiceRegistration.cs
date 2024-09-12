﻿using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OnionArchitecture.Application.Common.Behaviour;
using OnionArchitecture.Application.Features.Queries;
using OnionArchitecture.Infrastructure.Services;
using System.Reflection;

namespace OnionArchitecture.Application;
public static class ServiceRegistration
{
    public static void AddApplicationRegistration(this IServiceCollection services)
    { 
        services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehaviour<,>));

        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IUserQueries, UserQueries>();
    }
}
