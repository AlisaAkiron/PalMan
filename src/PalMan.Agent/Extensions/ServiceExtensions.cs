using PalMan.Agent.Jobs;
using PalMan.Agent.Queue;
using PalMan.Agent.Queue.Abstract;
using PalMan.Agent.Queue.Executor;
using PalMan.Agent.Queue.Models;

namespace PalMan.Agent.Extensions;

public static class ServiceExtensions
{
    public static void AddTaskQueue(this IServiceCollection services)
    {
        services.AddHostedService<TaskQueueExecutionJob>();

        services.AddSingleton<ITaskQueue, UpdateServerTaskQueue>();

        services.AddScoped<ITaskQueueExecutor<UpdateServerTask>, UpdateServerTaskExecutor>();
    }
}
