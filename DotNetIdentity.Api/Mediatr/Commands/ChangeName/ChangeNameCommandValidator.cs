using DotNetIdentity.Application.Core.Errors;
using DotNetIdentity.Application.Core.Extensions;
using FluentValidation;

namespace DotNetIdentity.Api.Mediatr.Commands.ChangeName;

/// <summary>
/// Represents the <see cref="ChangeNameCommand"/> validator class.
/// </summary>
public sealed class ChangeNameCommandValidator
    : AbstractValidator<ChangeNameCommand>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ChangeNameCommandValidator"/> class.
    /// </summary>
    public ChangeNameCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithError(ValidationErrors.ChangeName.UserIdIsRequired);

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithError(ValidationErrors.ChangeName.NameIsRequired);
        
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithError(ValidationErrors.ChangeName.NameIsRequired);
    }
}