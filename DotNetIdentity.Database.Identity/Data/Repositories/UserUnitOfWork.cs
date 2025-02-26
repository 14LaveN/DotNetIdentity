using DotNetIdentity.Database.Identity.Data.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace DotNetIdentity.Database.Identity.Data.Repositories;

/// <summary>
/// Represents the user unit of work.
/// </summary>
internal sealed class UserUnitOfWork(UserDbContext userDbContext)
    : IUserUnitOfWork
{
    /// <summary>
    /// Save changes async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The integer result of saving changes.</returns>
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await userDbContext.SaveChangesAsync(cancellationToken);

    /// <summary>
    /// Begin transaction async in your db context
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The db context transaction result of begin transaction.</returns>
    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
        await userDbContext.Database.BeginTransactionAsync(cancellationToken);
}