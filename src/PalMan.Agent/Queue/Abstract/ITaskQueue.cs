namespace PalMan.Agent.Queue.Abstract;

public interface ITaskQueue
{
    public Task Run(CancellationToken token);
}
