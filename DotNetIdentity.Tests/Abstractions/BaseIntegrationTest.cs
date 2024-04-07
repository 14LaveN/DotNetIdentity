using Bogus;
using DotNetIdentity.Database.Identity;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetIdentity.Tests.Abstractions;

public abstract class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
    private readonly IServiceScope _scope;

    protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
    {
        _scope = factory.Services.CreateScope();
        DbContext = _scope.ServiceProvider.GetRequiredService<UserDbContext>();
        Sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        Faker = new Faker();
    }

    protected ISender Sender { get; }

    protected UserDbContext DbContext { get; }

    protected Faker Faker { get; }

    public void Dispose()
    {
        _scope.Dispose();
    }
}