namespace PalMan.Agent.Queue.Abstract;

public interface ITaskQueueExecutor<in T>
{
    public Task Execute(T data, CancellationToken token);
}
