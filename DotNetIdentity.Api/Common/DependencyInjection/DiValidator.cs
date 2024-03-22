using DotNetIdentity.Api.Mediatr.Commands.ChangePassword;
using DotNetIdentity.Api.Mediatr.Commands.Login;
using DotNetIdentity.Api.Mediatr.Commands.Register;
using FluentValidation;

namespace DotNetIdentity.Api.Common.DependencyInjection;

public static class DiValidator
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IValidator<ChangePasswordCommand>, ChangePasswordCommandValidator>();
        services.AddScoped<IValidator<LoginCommand>, LoginCommandValidator>();
        services.AddScoped<IValidator<RegisterCommand>, RegisterCommandValidator>();
        
        return services;
    }
}