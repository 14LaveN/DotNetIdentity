using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Database.Identity.Data.Repositories;
using MediatR.NotificationPublishers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace DotNetIdentity.Database.Identity;

public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddUserDatabase(this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }
        
        var connectionString = configuration.GetConnectionString("DNIGenericDb");

        services.AddMediatR(x =>
        {
            x.RegisterServicesFromAssemblyContaining<Program>();

            x.NotificationPublisher = new TaskWhenAllPublisher();
        });
        
        services.AddDbContext<UserDbContext>(o => 
            o.UseNpgsql(connectionString, act 
                    =>
                {
                    act.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    act.EnableRetryOnFailure(3);
                    act.CommandTimeout(30);
                })
                .LogTo(Console.WriteLine)
                .EnableServiceProviderCaching()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors());
        
        if (connectionString is not null)
            services.AddHealthChecks()
                .AddNpgSql(connectionString);
        
        services.AddScoped<IUserUnitOfWork, UserUnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}