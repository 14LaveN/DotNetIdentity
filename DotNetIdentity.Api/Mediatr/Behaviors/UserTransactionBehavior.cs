using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Database.Identity;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DotNetIdentity.Api.Mediatr.Behaviors;

/// <summary>
/// Represents the <see cref="User"/> transaction behaviour class.
/// </summary>
internal sealed class UserTransactionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
    where TResponse : class
{
    private readonly IUserUnitOfWork _unitOfWork;
    private readonly UserDbContext _userDbContext;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserTransactionBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The user unit of work.</param>
    /// <param name="userDbContext">The user database context.</param>
    public UserTransactionBehavior(
        IUserUnitOfWork unitOfWork,
        UserDbContext userDbContext)
    {
        _unitOfWork = unitOfWork;
        _userDbContext = userDbContext;
    }

    /// <inheritdoc/>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is IQuery<TResponse>)
        {
            return await next();
        }
        
        var strategy = _userDbContext.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                TResponse response = await next();

                await transaction.CommitAsync(cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return response;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync(cancellationToken);

                throw;
            }
        });

        throw new ArgumentException();
    }
}