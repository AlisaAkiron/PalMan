using System.Collections.Concurrent;

namespace PalMan.Agent.Queue.Abstract;

public abstract class BaseTaskQueue<T> : ITaskQueue
{
    private readonly IServiceProvider _serviceProvider;

    protected BaseTaskQueue(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    private readonly ConcurrentQueue<T> _queue = [];

    public void Add(T item)
    {
        _queue.Enqueue(item);
    }

    public async Task Run(CancellationToken token)
    {
        while (token.IsCancellationRequested is false)
        {
            var success = _queue.TryDequeue(out var next);
            if (success is false || next is null)
            {
                await Task.Delay(2000, token);
                continue;
            }

            var scope = _serviceProvider.CreateAsyncScope();
            var executor = scope.ServiceProvider.GetRequiredService<ITaskQueueExecutor<T>>();

            await executor.Execute(next, token);

            await scope.DisposeAsync();
        }
    }
}
