using System.ComponentModel.DataAnnotations.Schema;
using DotNetIdentity.Domain.Common.Core.Primitives;
using DotNetIdentity.Domain.Core.Abstractions;
using DotNetIdentity.Domain.Core.Errors;
using DotNetIdentity.Domain.Core.Utility;
using DotNetIdentity.Domain.Core.Events;
using DotNetIdentity.Domain.Core.Primitives;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.Events.User;
using DotNetIdentity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace DotNetIdentity.Domain.Entities;

/// <summary>
/// Represents the user entity.
/// </summary>
public sealed class User : IdentityUser<Guid>, IAuditableEntity, ISoftDeletableEntity
{
    public override string? PasswordHash { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="firstName">The user first name.</param>
    /// <param name="lastName">The user last name.</param>
    /// <param name="emailAddress">The user emailAddress instance.</param>
    /// <param name="passwordHash">The user password hash.</param>
    public User(
        FirstName firstName,
        LastName lastName,
        EmailAddress emailAddress,
        string passwordHash)
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
        Ensure.NotEmpty(emailAddress, "The emailAddress is required.", nameof(emailAddress));
        Ensure.NotEmpty(passwordHash, "The password hash is required", nameof(passwordHash));

        FirstName = firstName;
        LastName = lastName;
        EmailAddress = emailAddress;
        PasswordHash = passwordHash;
    }

    /// <inheritdoc />
    private User() { }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public override Guid Id { get; set; }
    
    /// <summary>
    /// Gets the user first name.
    /// </summary>
    public override string? UserName { get; set; }
    
    /// <summary>
    /// Gets the user first name.
    /// </summary>
    public FirstName FirstName { get; private set; }

    /// <summary>
    /// Gets the user last name.
    /// </summary>
    public LastName LastName { get; private set; }
    
    /// <summary>
    /// Gets the user full name.
    /// </summary>
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets the user emailAddress.
    /// </summary>
    public EmailAddress EmailAddress { get; set; }

    /// <inheritdoc />
    public DateTime CreatedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? ModifiedOnUtc { get; }

    /// <inheritdoc />
    public DateTime? DeletedOnUtc { get; }

    /// <inheritdoc />
    public bool Deleted { get; }

    /// <inheritdoc cref="RefreshToken" />
    public string? RefreshToken { get; set; }

    /// <summary>
    /// The domain events.
    /// </summary>
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// Gets the domain events. This collection is readonly.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Gets or sets first name.
    /// </summary>
    public string? Firstname { get; set; }

    /// <summary>
    /// Clears all the domain events from the <see cref="AggregateRoot"/>.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();
        
    /// <summary>
    /// Adds the specified <see cref="IDomainEvent"/> to the <see cref="AggregateRoot"/>.
    /// </summary>
    /// <param name="domainEvent">The domain event.</param>
    private void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <summary>
    /// Creates a new user with the specified first name, last name, emailAddress and password hash.
    /// </summary>
    /// <param name="firstName">The first name.</param>
    /// <param name="lastName">The last name.</param>
    /// <param name="emailAddress">The emailAddress.</param>
    /// <param name="passwordHash">The password hash.</param>
    /// <returns>The newly created user instance.</returns>
    public static User Create(
        FirstName firstName,
        LastName lastName,
        EmailAddress emailAddress,
        string passwordHash)
    {
        var user = new User(firstName, lastName, emailAddress, passwordHash);
        
        user.AddDomainEvent(new UserCreatedDomainEvent(user));

        return user;
    }

    /// <summary>
    /// Changes the users password.
    /// </summary>
    /// <param name="passwordHash">The password hash of the new password.</param>
    /// <returns>The success result or an error.</returns>
    public Result ChangePassword(string passwordHash)
    {
        if (passwordHash == PasswordHash)
        {
            return Result.Failure(DomainErrors.User.CannotChangePassword);
        }

        PasswordHash = passwordHash;

        AddDomainEvent(new UserPasswordChangedDomainEvent(this));

        return Result.Success().GetAwaiter().GetResult();
    }

    /// <summary>
    /// Changes the users first and last name.
    /// </summary>
    /// <param name="firstName">The new first name.</param>
    /// <param name="lastName">The new last name.</param>
    public void ChangeName(FirstName firstName, LastName lastName)
    {
        Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
        Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));

        FirstName = firstName;

        LastName = lastName;

        AddDomainEvent(new UserNameChangedDomainEvent(this));
    }
}