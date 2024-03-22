using DotNetIdentity.Api.Mediatr.Commands.ChangeName;
using DotNetIdentity.Application.ApiHelpers.Responses;
using DotNetIdentity.Application.Core.Abstractions.Messaging;
using DotNetIdentity.Database.Identity.Data.Interfaces;
using DotNetIdentity.Domain.Core.Errors;
using DotNetIdentity.Domain.Core.Primitives.Maybe;
using DotNetIdentity.Domain.Core.Primitives.Result;
using DotNetIdentity.Domain.Entities;
using DotNetIdentity.Domain.Enumerations;
using DotNetIdentity.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace DotNetIdentity.Api.Mediatr.Commands.ChangeName;

/// <summary>
/// Represents the <see cref="ChangeNameCommand"/> handler.
/// </summary>
internal sealed class ChangeNameCommandHandler : ICommandHandler<ChangeNameCommand, IBaseResponse<Result>>
{
    private readonly IUserUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeNameCommandHandler"/> class.
    /// </summary>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="userManager"></param>
    public ChangeNameCommandHandler(
        IUserUnitOfWork unitOfWork,
        UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }

    /// <inheritdoc />
    public async Task<IBaseResponse<Result>> Handle(ChangeNameCommand request, CancellationToken cancellationToken)
    {
        Result<FirstName> nameResult = FirstName.Create(request.FirstName);

        if (nameResult.IsFailure)
        {
            return new BaseResponse<Result>
            {
                Data = Result.Failure(nameResult.Error),
                StatusCode = StatusCode.InternalServerError,
                Description = "First Name result is failure."
            };
        }
        
        Result<LastName> lastNameResult = LastName.Create(request.LastName);

        if (lastNameResult.IsFailure)
        {
            return new BaseResponse<Result>
            {
                Data = Result.Failure(lastNameResult.Error),
                StatusCode = StatusCode.InternalServerError,
                Description = "Last Name result is failure."
            };
        }
        
        Maybe<User> maybeUser = await _userManager.FindByIdAsync(request.UserId.ToString()) 
                                ?? throw new ArgumentException();

        if (maybeUser.HasNoValue)
        {
            return new BaseResponse<Result>
            {
                Data = Result.Failure(DomainErrors.User.NotFound),
                StatusCode = StatusCode.NotFound,
                Description = "User not found."
            };
        }

        User user = maybeUser.Value;

        user.ChangeName(request.FirstName,request.LastName);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new BaseResponse<Result>
        {
            Data = Result.Success(),
            Description = "Change name.",
            StatusCode = StatusCode.Ok
        };
    }
}