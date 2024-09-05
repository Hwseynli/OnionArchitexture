using System;
using System.Reflection;
using MediatR;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using OnionArchitecture.Application.Common.Behaviour;

namespace OnionArchitecture.Application;
public static class ServiceRegistration
{
    public static void AddApplicationRegistrantion(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>),typeof(ValidationBehavior<,>));
    }
}

