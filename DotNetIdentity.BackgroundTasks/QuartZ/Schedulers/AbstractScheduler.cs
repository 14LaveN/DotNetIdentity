using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using DotNetIdentity.BackgroundTasks.QuartZ.Jobs;

namespace DotNetIdentity.BackgroundTasks.QuartZ.Schedulers;

/// <summary>
/// Represents the abstract scheduler class.
/// </summary>
/// <typeparam name="T">The generic job type.</typeparam>
public abstract class AbstractScheduler<T> 
    where T: IJob
{
    /// <summary>
    /// Starts the job.
    /// </summary>
    /// <param name="serviceProvider">The service provider.</param>
    public virtual async void Start(IServiceCollection serviceProvider)
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
            //scheduler.JobFactory = serviceProvider.GetService<QuartzJobFactory>();
        await scheduler.Start();

        IJobDetail jobDetail = JobBuilder.Create<UserDbJob>().Build();
        ITrigger trigger = TriggerBuilder
            .Create()
            .WithIdentity($"{nameof(UserDbJob)}Trigger", "default")
            .StartAt(DateTimeOffset.UtcNow.Add(TimeSpan.FromMinutes(1)))
            .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(160)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(jobDetail, trigger);
    }
}