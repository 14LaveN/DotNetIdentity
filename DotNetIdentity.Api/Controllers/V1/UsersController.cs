using DotNetIdentity.Api.Contracts.Users.ChangeName;
using DotNetIdentity.Application.ApiHelpers.Contracts;
using DotNetIdentity.Application.ApiHelpers.Infrastructure;
using DotNetIdentity.Application.ApiHelpers.Policy;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Api.Contracts.Users.Login;
using DotNetIdentity.Api.Contracts.Users.Register;
using DotNetIdentity.Api.Mediatr.Commands.ChangeName;
using DotNetIdentity.Api.Mediatr.Commands.ChangePassword;
using DotNetIdentity.Api.Mediatr.Commands.Login;
using DotNetIdentity.Api.Mediatr.Commands.Register;
using DotNetIdentity.Domain.Core.Errors;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotNetIdentity.Api.Controllers.V1;

/// <summary>
/// Represents the users controller class.
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="userRepository">The user repository.</param>
[Route("api/v1/users")]
public sealed class UsersController(
        ISender sender,
        IUserRepository userRepository)
    : ApiController(sender, userRepository, nameof(UsersController))
{
    #region Commands.
    
    /// <summary>
    /// Login user.
    /// </summary>
    /// <param name="request">The <see cref="LoginRequest"/> class.</param>
    /// <returns>Base information about login user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Login)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(loginRequest => new LoginCommand(loginRequest.UserName,Password.Create(loginRequest.Password).Value))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);
    
    /// <summary>
    /// Register user.
    /// </summary>
    /// <param name="request">The <see cref="RegisterRequest"/> class.</param>
    /// <returns>Base information about register user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="401">Unauthorized.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.Register)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(registerRequest => new RegisterCommand(
                    FirstName.Create(registerRequest.FirstName).Value,
                    LastName.Create(registerRequest.LastName).Value,
                    new EmailAddress(registerRequest.Email),
                    Password.Create(registerRequest.Password).Value,
                    registerRequest.UserName))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, Unauthorized);

    /// <summary>
    /// Change password from user.
    /// </summary>
    /// <param name="password">The password.</param>
    /// <returns>Base information about change password from user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.ChangePassword)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] string password) =>
        await Result.Create(password, DomainErrors.General.UnProcessableRequest)
            .Map(changePasswordRequest => new ChangePasswordCommand(UserId,changePasswordRequest))
            .Bind(command => BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)))
            .Match(Ok, BadRequest);
    
    /// <summary>
    /// Change name from user.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns>Base information about change name from user method.</returns>
    /// <remarks>
    /// Example request:
    /// </remarks>
    /// <response code="200">OK.</response>
    /// <response code="400">Bad request.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost(ApiRoutes.Users.ChangeName)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangeNameRequest request) =>
        await Result.Create(request, DomainErrors.General.UnProcessableRequest)
            .Map(changeNameRequest => new ChangeNameCommand(
                FirstName.Create(changeNameRequest.FirstName).Value,
                LastName.Create(changeNameRequest.LastName).Value,
                UserId))
            .Bind(async command => await BaseRetryPolicy.Policy.Execute(async () =>
                await Sender.Send(command)).Result.Data)
            .Match(Ok, BadRequest);
    
    #endregion
}