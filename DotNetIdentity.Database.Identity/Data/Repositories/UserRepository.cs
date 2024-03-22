using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Domain.Core.Primitives.Maybe;
using DotNetIdentity.Domain.Entities;
using DotNetIdentity.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DotNetIdentity.Database.Identity.Data.Repositories;

/// <summary>
/// Represents the <see cref="User"/> entity repository class.
/// </summary>
/// <param name="userDbContext">The user database context.</param>
public sealed class UserRepository(UserDbContext userDbContext)
    : IUserRepository
{
    /// <inheritdoc />
    public async Task<Maybe<User>> GetByIdAsync(Guid userId) =>
            await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.Id == userId) 
            ?? throw new ArgumentNullException();

    /// <inheritdoc />
    public async Task<Maybe<User>> GetByNameAsync(string name) =>
        await userDbContext.Set<User>().FirstOrDefaultAsync(x=>x.UserName == name) 
        ?? throw new ArgumentNullException();

    /// <inheritdoc />
    public async Task<Maybe<User>> GetByEmailAsync(EmailAddress emailAddress) =>
        await userDbContext.Set<User>().SingleOrDefaultAsync(x=>x.EmailAddress == emailAddress) 
        ?? throw new ArgumentNullException();
}