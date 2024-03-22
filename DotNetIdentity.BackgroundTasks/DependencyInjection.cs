using System.Reflection;
using Microsoft.Extensions.Configuration;
using DotNetIdentity.BackgroundTasks.QuartZ;
using DotNetIdentity.BackgroundTasks.QuartZ.Jobs;
using DotNetIdentity.BackgroundTasks.QuartZ.Schedulers;
using DotNetIdentity.BackgroundTasks.Services;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace DotNetIdentity.BackgroundTasks;

public static class BDependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddBackgroundTasks(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(x=>
            x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        services.AddScoped<IIntegrationEventConsumer, IntegrationEventConsumer>();

        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(UserDbJob));

            configure
                .AddJob<UserDbJob>(jobKey);
            
            configure.UseMicrosoftDependencyInjectionJobFactory();
        });

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });
        
        services.AddTransient<IJobFactory, QuartzJobFactory>();
        services.AddSingleton(_ =>
        {
            var schedulerFactory = new StdSchedulerFactory();
            var scheduler = schedulerFactory.GetScheduler().Result;
            
            return scheduler;
        });
        services.AddTransient<UserDbScheduler>();
        
        var scheduler = new UserDbScheduler();
        scheduler.Start(services);
        
        return services;
    }
}