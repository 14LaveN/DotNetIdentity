using DotNetIdentity.Api.Mediatr.Behaviors;
using DotNetIdentity.Api.Mediatr.Commands.ChangeName;
using DotNetIdentity.Application.Core.Behaviours;
using DotNetIdentity.Api.Mediatr.Commands.ChangePassword;
using DotNetIdentity.Api.Mediatr.Commands.Login;
using DotNetIdentity.Api.Mediatr.Commands.Register;
using MediatR;
using MediatR.NotificationPublishers;

namespace DotNetIdentity.Api.Common.DependencyInjection;

public static class DiMediator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddMediatr(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();

            x.RegisterServicesFromAssemblies(typeof(RegisterCommand).Assembly,
                typeof(RegisterCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(LoginCommand).Assembly,
                typeof(LoginCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ChangePasswordCommand).Assembly,
                typeof(ChangePasswordCommandHandler).Assembly);
            
            x.RegisterServicesFromAssemblies(typeof(ChangeNameCommand).Assembly,
                typeof(ChangeNameCommandHandler).Assembly);
            
            x.AddOpenBehavior(typeof(QueryCachingBehavior<,>));
            x.AddOpenBehavior(typeof(UserTransactionBehavior<,>));
            x.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            x.AddOpenBehavior(typeof(MetricsBehaviour<,>));
            
            x.NotificationPublisher = new TaskWhenAllPublisher();
        });
        
        return services;
    }
}