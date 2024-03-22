namespace DotNetIdentity.Api.Contracts.Users.Get;

public sealed record GetUserResponse(
    string Description,
    string Name,
    DateTime CreatedAt);