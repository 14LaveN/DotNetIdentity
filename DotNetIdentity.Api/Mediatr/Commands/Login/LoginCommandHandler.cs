using DotNetIdentity.Application.ApiHelpers.Responses;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Application.Core.Settings.User;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Api.Extensions;
using DotNetIdentity.Application.Core.Abstractions.Helpers.JWT;
using DotNetIdentity.Domain.Core.Exceptions;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.Entities;
using DotNetIdentity.Domain.Enumerations;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DotNetIdentity.Api.Mediatr.Commands.Login;

/// <summary>
/// Represents the login command handler class.
/// </summary>
/// <param name="logger">The logger.</param>
/// <param name="userManager">The user manager.</param>
/// <param name="jwtOptions">The json web token options.</param>
/// <param name="signInManager">The sign in manager.</param>
internal sealed class LoginCommandHandler(
        ILogger<LoginCommandHandler> logger,
        UserManager<User> userManager,
        IOptions<JwtOptions> jwtOptions,
        SignInManager<User> signInManager)
    : ICommandHandler<LoginCommand, LoginResponse<Result>>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;
    private readonly SignInManager<User> _signInManager = signInManager ?? throw new ArgumentNullException();
    
    /// <inheritdoc />
    public async Task<LoginResponse<Result>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation($"Request for login an account - {request.UserName}");
            
            var user = await userManager.FindByNameAsync(request.UserName);

            if (user is null)
            {
                logger.LogWarning("User with the same name not found");
                throw new NotFoundException(nameof(user), "User with the same name");
            }

            if (!await userManager.CheckPasswordAsync(user, request.Password))
            {
                logger.LogWarning("The password does not meet the assessment criteria");
                throw new AuthenticationException();
            }
            
            var result = await _signInManager.PasswordSignInAsync(request.UserName,
                request.Password, false, false);

            var (refreshToken, refreshTokenExpireAt) = user.GenerateRefreshToken(_jwtOptions);
            
            if (result.Succeeded)
            {
                user.RefreshToken = refreshToken;
            }
            
            logger.LogInformation($"User logged in - {user.UserName} {DateTime.UtcNow}");
            
            return new LoginResponse<Result>
            {
                Description = "Login account",
                StatusCode = StatusCode.Ok,
                Data = Result.Success(),
                AccessToken = user.GenerateAccessToken(_jwtOptions), 
                RefreshToken = refreshToken,
                RefreshTokenExpireAt = refreshTokenExpireAt
            };
        }
        catch (Exception exception)
        {
            logger.LogError(exception, $"[LoginCommandHandler]: {exception.Message}");
            throw new AuthenticationException(exception.Message);
        }
    }
}