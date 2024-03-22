using DotNetIdentity.Domain.Common.Core.Primitives;
using DotNetIdentity.Domain.Core.Primitives;

namespace DotNetIdentity.Domain.Core.Errors;

/// <summary>
/// Contains the domain errors.
/// </summary>
public static class DomainErrors
{
    /// <summary>
    /// Contains the user errors.
    /// </summary>
    public static class User
    {
        public static Error NotFound => new("User.NotFound", "The user with the specified identifier was not found.");

        public static Error InvalidPermissions => new(
            "User.InvalidPermissions",
            "The current user does not have the permissions to perform that operation.");
            
        public static Error DuplicateEmail => new("User.DuplicateEmail", "The specified emailAddress is already in use.");

        public static Error CannotChangePassword => new(
            "User.CannotChangePassword",
            "The password cannot be changed to the specified password.");
    }

    /// <summary>
    /// Contains the event errors.
    /// </summary>
    public static class Event
    {
        public static Error AlreadyCancelled => new("Event.AlreadyCancelled", "The event has already been cancelled.");

        public static Error EventHasPassed => new(
            "Event.EventHasPassed",
            "The event has already passed and cannot be modified.");
    }
    
    /// <summary>
    /// Contains the name errors.
    /// </summary>
    public static class Name
    {
        public static Error NullOrEmpty => new("Name.NullOrEmpty", "The name is required.");

        public static Error LongerThanAllowed => new("Name.LongerThanAllowed", "The name is longer than allowed.");
    }

    /// <summary>
    /// Contains the first name errors.
    /// </summary>
    public static class FirstName
    {
        public static Error NullOrEmpty => new("FirstName.NullOrEmpty", "The first name is required.");

        public static Error LongerThanAllowed => new("FirstName.LongerThanAllowed", "The first name is longer than allowed.");
    }

    /// <summary>
    /// Contains the last name errors.
    /// </summary>
    public static class LastName
    {
        public static Error NullOrEmpty => new("LastName.NullOrEmpty", "The last name is required.");

        public static Error LongerThanAllowed => new("LastName.LongerThanAllowed", "The last name is longer than allowed.");
    }

    /// <summary>
    /// Contains the emailAddress errors.
    /// </summary>
    public static class Email
    {
        public static Error NullOrEmpty => new("EmailAddress.NullOrEmpty", "The emailAddress is required.");

        public static Error LongerThanAllowed => new("EmailAddress.LongerThanAllowed", "The emailAddress is longer than allowed.");

        public static Error InvalidFormat => new("EmailAddress.InvalidFormat", "The emailAddress format is invalid.");
    }

    /// <summary>
    /// Contains the password errors.
    /// </summary>
    public static class Password
    {
        public static Error NullOrEmpty => new("Password.NullOrEmpty", "The password is required.");

        public static Error TooShort => new("Password.TooShort", "The password is too short.");

        public static Error MissingUppercaseLetter => new(
            "Password.MissingUppercaseLetter",
            "The password requires at least one uppercase letter.");

        public static Error MissingLowercaseLetter => new(
            "Password.MissingLowercaseLetter",
            "The password requires at least one lowercase letter.");

        public static Error MissingDigit => new(
            "Password.MissingDigit",
            "The password requires at least one digit.");

        public static Error MissingNonAlphaNumeric => new(
            "Password.MissingNonAlphaNumeric",
            "The password requires at least one non-alphanumeric.");
    }

    /// <summary>
    /// Contains general errors.
    /// </summary>
    public static class General
    {
        public static Error UnProcessableRequest => new(
            "General.UnProcessableRequest",
            "The server could not process the request.");

        public static Error ServerError => new("General.ServerError", "The server encountered an unrecoverable error.");
    }

    /// <summary>
    /// Contains the authentication errors.
    /// </summary>
    public static class Authentication
    {
        public static Error InvalidEmailOrPassword => new(
            "Authentication.InvalidEmailOrPassword",
            "The specified emailAddress or password are incorrect.");
    }
}