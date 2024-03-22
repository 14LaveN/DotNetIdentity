using Microsoft.Extensions.DependencyInjection;
using DotNetIdentity.Application.Common;
using DotNetIdentity.Application.Core.Abstractions.Common;
using DotNetIdentity.Application.Core.Abstractions.Helpers.JWT;
using DotNetIdentity.Application.Core.Helpers.JWT;
using DotNetIdentity.Application.Core.Helpers.Metric;

namespace DotNetIdentity.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        if (services is null)
            throw new ArgumentException();

        services.AddScoped<IUserIdentifierProvider, UserIdentifierProvider>();
        services.AddScoped<CreateMetricsHelper>();
        services.AddScoped<IDateTime, MachineDateTime>();
        
        return services;
    }
}