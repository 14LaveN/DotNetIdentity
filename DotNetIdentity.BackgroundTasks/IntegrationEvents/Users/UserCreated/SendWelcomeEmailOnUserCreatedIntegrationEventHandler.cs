using DotNetIdentity.Application.Core.Abstractions.Notifications;
using DotNetIdentity.BackgroundTasks.Abstractions.Messaging;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Domain.Core.Errors;
using DotNetIdentity.Domain.Core.Exceptions;
using DotNetIdentity.Domain.Core.Primitives.Maybe;
using DotNetIdentity.Domain.Entities;
using DotNetIdentity.Email.Contracts.Emails;
using DotNetIdentity.RabbitMq.Messaging.User.Events.UserCreated;

namespace DotNetIdentity.BackgroundTasks.IntegrationEvents.Users.UserCreated;

/// <summary>
/// Represents the <see cref="UserCreatedIntegrationEvent"/> handler.
/// </summary>
internal sealed class SendWelcomeEmailOnUserCreatedIntegrationEventHandler 
    : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailNotificationService _emailNotificationService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SendWelcomeEmailOnUserCreatedIntegrationEventHandler"/> class.
    /// </summary>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="emailNotificationService">The emailAddress notification service.</param>
    public SendWelcomeEmailOnUserCreatedIntegrationEventHandler(
        IUserRepository userRepository,
        IEmailNotificationService emailNotificationService)
    {
        _emailNotificationService = emailNotificationService;
        _userRepository = userRepository;
    }

    /// <inheritdoc />
    public async Task Handle(UserCreatedIntegrationEvent notification, CancellationToken cancellationToken)
    {
        Maybe<User> maybeUser = await _userRepository.GetByIdAsync(notification.UserId);

        if (maybeUser.HasNoValue)
        {
            throw new DomainException(DomainErrors.User.NotFound);
        }

        User user = maybeUser.Value;

        var welcomeEmail = new WelcomeEmail(user.EmailAddress, user.FullName);

        await _emailNotificationService.SendWelcomeEmail(welcomeEmail);
    }
}