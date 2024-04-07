using DotNetIdentity.BackgroundTasks.Abstractions.Messaging;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Domain.Core.Errors;
using DotNetIdentity.Domain.Core.Exceptions;
using DotNetIdentity.Domain.Core.Primitives.Maybe;
using DotNetIdentity.Domain.Entities;
using DotNetIdentity.Email.Contracts.Emails;
using DotNetIdentity.Email.Emails;
using DotNetIdentity.RabbitMq.Messaging.User.Events.PasswordChanged;

namespace DotNetIdentity.BackgroundTasks.IntegrationEvents.Users.UserPasswordChanged;

/// <summary>
/// Represents the <see cref="UserPasswordChangedIntegrationEvent"/> handler.
/// </summary>
internal sealed class NotifyUserOnPasswordChangedIntegrationEventHandler
    : IIntegrationEventHandler<UserPasswordChangedIntegrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotifyUserOnPasswordChangedIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public NotifyUserOnPasswordChangedIntegrationEventHandler(
        IUserRepository userRepository,
        IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task Handle(UserPasswordChangedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(notification.UserId);

        if (maybeUser.HasNoValue)
        {
            throw new DomainException(DomainErrors.User.NotFound);
        }

        User user = maybeUser.Value;

        var passwordChangedEmail = new PasswordChangedEmail(user.EmailAddress, user.FullName);

        await _emailNotificationService.SendPasswordChangedEmail(passwordChangedEmail);
    }
}