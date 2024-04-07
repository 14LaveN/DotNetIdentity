using DotNetIdentity.Database.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Testcontainers.Redis;

namespace DotNetIdentity.Tests.Abstractions;

public abstract class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly RedisContainer _redisContainer = new RedisBuilder()
        .WithImage("redis:7.0")
        .Build();
    
    public async Task InitializeAsync()
    {
        await _redisContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<UserDbContext>));
            
            services.AddDbContext<UserDbContext>(o => 
                o.UseNpgsql("Server=localhost;Port=5433;Database=DNIGenericDb;User Id=postgres;Password=1111;", act 
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

            services.RemoveAll(typeof(RedisCacheOptions));

            services.AddStackExchangeRedisCache(options =>
                options.Configuration = _redisContainer.GetConnectionString());
        });
    }

    public new async Task DisposeAsync()
    {
        await _redisContainer.StopAsync();
    }
}