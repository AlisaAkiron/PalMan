using PalMan.Agent.Queue.Abstract;

namespace PalMan.Agent.Jobs;

public class TaskQueueExecutionJob : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    private readonly List<Task> _tasks = [];

    public TaskQueueExecutionJob(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var taskQueues = _serviceProvider
            .GetServices<ITaskQueue>()
            .ToList();

        foreach (var taskQueue in taskQueues)
        {
            _tasks.Add(taskQueue.Run(stoppingToken));
        }

        await Task.WhenAll(_tasks);
    }
}
