using DotNetIdentity.Api.Contracts.Users.Get;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Domain.Core.Primitives.Maybe;

namespace DotNetIdentity.Api.Mediatr.Queries.GetTheUserById;

/// <summary>
/// Represents the get user by id query record.
/// </summary>
/// <param name="UserId">The user identifier.</param>
public sealed record GetTheUserByIdQuery(Guid UserId)
    : ICachedQuery<Maybe<List<GetUserResponse>>>
{
    public string Key { get; } = $"get-user-by-{UserId}";
    
    public TimeSpan? Expiration { get; } = TimeSpan.FromMinutes(6);
}