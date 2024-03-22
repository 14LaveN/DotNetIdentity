namespace DotNetIdentity.Domain.Core.Exceptions;

/// <summary>
/// Represents the not found exception class.
/// </summary>
public sealed class NotFoundException : Exception
{
    /// <summary>
    /// Create the new instance of <see cref="NotFoundException"/> class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="key">The key.</param>
    public NotFoundException(string name, object key)
        : base($"Entity {name} ({key}) not found.") { }
}