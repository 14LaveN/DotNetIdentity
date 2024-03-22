using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using DotNetIdentity.BackgroundTasks.QuartZ.Jobs;

namespace DotNetIdentity.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the user database scheduler class.
/// </summary>
public sealed class UserDbScheduler
    : AbstractScheduler<UserDbJob>;