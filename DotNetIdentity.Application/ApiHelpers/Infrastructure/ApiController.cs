using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DotNetIdentity.Application.ApiHelpers.Contracts;
using DotNetIdentity.Application.ApiHelpers.Policy;
using DotNetIdentity.Application.Core.Helpers.JWT;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Domain.Core.Primitives;
using DotNetIdentity.Domain.Core.Primitives.Maybe;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.Entities;

namespace DotNetIdentity.Application.ApiHelpers.Infrastructure;

/// <summary>
/// Represents the api controller class.
/// </summary>
[ApiController]
[Produces("application/json")]
[ApiExplorerSettings(GroupName = "v1")]
public class ApiController : ControllerBase
{
    protected ApiController(
        ISender sender,
        IUserRepository userRepository, string controllerName)
    {
        Sender = sender;
        UserRepository = userRepository;
        ControllerName = controllerName;
    }

    protected string ControllerName { get; }

    protected ISender Sender { get; }

    protected Maybe<Guid> UserId { get; }

    protected IUserRepository UserRepository { get; }

    [HttpGet("get-profile-by-id")]
    public async Task<Maybe<User>> GetProfileById(Guid authorId)
    {
        var profile = 
            await UserRepository.GetByIdAsync(authorId);

        return profile;
    }
        
    /// <summary>
    /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
    /// response based on the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
    protected IActionResult BadRequest(Error error) => BadRequest(new ApiErrorResponse(new[] { error }));
    
    /// <summary>
    /// Creates an <see cref="BadRequestObjectResult"/> that produces a <see cref="StatusCodes.Status400BadRequest"/>.
    /// response based on the specified <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error.</param>
    /// <returns>The created <see cref="BadRequestObjectResult"/> for the response.</returns>
    protected IActionResult Unauthorized(Error error) => Unauthorized(new ApiErrorResponse(new[] { error }));

    /// <summary>
    /// Creates an <see cref="OkObjectResult"/> that produces a <see cref="StatusCodes.Status200OK"/>.
    /// </summary>
    /// <returns>The created <see cref="OkObjectResult"/> for the response.</returns>
    /// <returns></returns>
    protected new IActionResult Ok(object value) => base.Ok(value);

    /// <summary>
    /// Creates an <see cref="NotFoundResult"/> that produces a <see cref="StatusCodes.Status404NotFound"/>.
    /// </summary>
    /// <returns>The created <see cref="NotFoundResult"/> for the response.</returns>
    protected new IActionResult NotFound() => base.NotFound();
}