using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using DotNetIdentity.BackgroundTasks.Services;
using DotNetIdentity.BackgroundTasks.Tasks;

namespace DotNetIdentity.BackgroundTasks;

public static class DiIntegrationEvent
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddRabbitBackgroundTasks(
        this IServiceCollection services)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddHostedService<IntegrationEventConsumerBackgroundService>();

        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        return services;
    }
}