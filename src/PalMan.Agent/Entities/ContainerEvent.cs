namespace PalMan.Agent.Entities;

public class ContainerEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset Time { get; set; } = DateTimeOffset.UtcNow;

    public string Message { get; set; } = string.Empty;
}
