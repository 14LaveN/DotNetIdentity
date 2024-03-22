using DotNetIdentity.Application.ApiHelpers.Responses;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.ValueObjects;

namespace DotNetIdentity.Api.Mediatr.Commands.Register;

/// <summary>
/// Represents the register command record class.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
/// <param name="Email">The email.</param>
/// <param name="Password">The password.</param>
/// <param name="UserName">The user name.</param>
public sealed record RegisterCommand(
    FirstName FirstName,
    LastName LastName,
    EmailAddress Email,
    Password Password,
    string UserName)
    : ICommand<LoginResponse<Result>>;