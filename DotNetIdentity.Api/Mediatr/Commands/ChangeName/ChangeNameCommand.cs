using DotNetIdentity.Application.ApiHelpers.Responses;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.ValueObjects;

namespace DotNetIdentity.Api.Mediatr.Commands.ChangeName;

/// <summary>
/// Represents the change <see cref="Name"/> command record.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="UserId">The user identifier.</param>
public sealed record ChangeNameCommand(
    FirstName FirstName,
    LastName LastName,
    Guid UserId)
    : ICommand<IBaseResponse<Result>>;