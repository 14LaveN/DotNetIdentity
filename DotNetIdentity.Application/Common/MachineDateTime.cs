﻿using DotNetIdentity.Application.Core.Abstractions.Common;

namespace DotNetIdentity.Application.Common;

/// <summary>
/// Represents the machine date time service.
/// </summary>
internal sealed class MachineDateTime : IDateTime
{
    /// <inheritdoc />
    public DateTime UtcNow => DateTime.UtcNow;
}