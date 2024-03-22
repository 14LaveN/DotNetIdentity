using DotNetIdentity.Api.Contracts.Users.Get;
using DotNetIdentity.Application.Core.Abstractions.Helpers.JWT;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Domain.Core.Primitives.Maybe;

namespace DotNetIdentity.Api.Mediatr.Queries.GetTheUserById;

/// <summary>
/// Represents the <see cref="GetTheUserByIdQuery"/> handler.
/// </summary>
/// <param name="userRepository">The user repository.</param>
/// <param name="userIdentifierProvider">The user identifier provider.</param>
/// <param name="logger">The logger.</param>
public sealed class GetTheUserByIdQueryHandler(
        IUserRepository userRepository,
        IUserIdentifierProvider userIdentifierProvider,
        ILogger<GetTheUserByIdQueryHandler> logger)
    : IQueryHandler<GetTheUserByIdQuery, Maybe<List<GetUserResponse>>>
{
    /// <inheritdoc />
    public Task<Maybe<List<GetUserResponse>>> Handle(
        GetTheUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}