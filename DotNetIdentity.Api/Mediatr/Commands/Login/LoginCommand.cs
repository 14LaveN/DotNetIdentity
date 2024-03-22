using DotNetIdentity.Application.ApiHelpers.Responses;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.ValueObjects;

namespace DotNetIdentity.Api.Mediatr.Commands.Login;

/// <summary>
/// Represents the login command record class.
/// </summary>
/// <param name="UserName">The user name.</param>
/// <param name="Password">The password.</param>
public sealed record LoginCommand(
        string UserName,
        Password Password)
    : ICommand<LoginResponse<Result>>;