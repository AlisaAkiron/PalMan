namespace PalMan.Agent.Queue.Abstract;

public class BaseTask
{
    public Guid TaskId { get; set; } = Guid.NewGuid();
}
