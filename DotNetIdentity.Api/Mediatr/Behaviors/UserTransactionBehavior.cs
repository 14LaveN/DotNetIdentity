using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using MediatR;

namespace DotNetIdentity.Api.Mediatr.Behaviors;

/// <summary>
/// Represents the <see cref="User"/> transaction behaviour class.
/// </summary>
internal sealed class UserTransactionBehavior<TRequest, TResponse> 
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IRequest<TResponse>
    where TResponse : class
{
    private readonly IUserUnitOfWork _unitOfWork;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserTransactionBehavior{TRequest,TResponse}"/> class.
    /// </summary>
    /// <param name="unitOfWork">The user unit of work.</param>
    public UserTransactionBehavior(IUserUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

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
    }
}