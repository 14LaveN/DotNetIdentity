using DotNetIdentity.Domain.Common.Core.Primitives;
using DotNetIdentity.Domain.Core.Primitives;

namespace DotNetIdentity.Application.ApiHelpers.Contracts;

/// <summary>
/// Represents API an error response.
/// </summary>
public class ApiErrorResponse
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiErrorResponse"/> class.
    /// </summary>
    /// <param name="errors">The enumerable collection of errors.</param>
    public ApiErrorResponse(IReadOnlyCollection<Error> errors) => Errors = errors;

    /// <summary>
    /// Gets the errors.
    /// </summary>
    public IReadOnlyCollection<Error> Errors { get; }
}