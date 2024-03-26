using DotNetIdentity.Domain.Events.User;
using DotNetIdentity.RabbitMq.Messaging;
using DotNetIdentity.RabbitMq.Messaging.Settings;
using DotNetIdentity.RabbitMq.Messaging.User.Events.UserCreated;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetIdentity.RabbitMq;

public static class DependencyInjection
{
    public static IServiceCollection AddRabbitMq(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
            x.RegisterServicesFromAssemblies(typeof(UserCreatedDomainEvent).Assembly,
                typeof(PublishIntegrationEventOnUserCreatedDomainEventHandler).Assembly));
        
        services.AddScoped<IIntegrationEventPublisher, IntegrationEventPublisher>();

        services.AddHealthChecks()
            .AddRabbitMQ();
        
        return services; 
    }
}