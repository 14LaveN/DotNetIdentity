using MediatR;
using Microsoft.Extensions.Caching.Memory;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Cache.Service;

namespace DotNetIdentity.Application.Core.Behaviours;

/// <summary>
/// Represents the transaction behaviour middleware.
/// </summary>
/// <typeparam name="TRequest">The request type.</typeparam>
/// <typeparam name="TResponse">The response type.</typeparam>
public sealed class QueryCachingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICachedQuery
    where TResponse : class
{
    private readonly IMemoryCache _cache;

    /// <summary>
    /// Initializes a new instance of the <see cref="QueryCachingBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="cache">The memory cache.</param>
    public QueryCachingBehavior(IMemoryCache cache)
    {
        _cache = cache;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken) =>
        await _cache.GetOrCreateAsync(request.Key,
            _ =>next(),
            request.Expiration,
            cancellationToken);
}