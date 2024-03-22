namespace DotNetIdentity.Api.Contracts.Users.ChangeName;

/// <summary>
/// Represents the change name request.
/// </summary>
/// <param name="FirstName">The first name.</param>
/// <param name="LastName">The last name.</param>
public sealed record ChangeNameRequest(
    string FirstName,
    string LastName);