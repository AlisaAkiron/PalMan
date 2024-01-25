using PalMan.Agent.Queue.Abstract;
using PalMan.Agent.Queue.Models;

namespace PalMan.Agent.Queue;

public class UpdateServerTaskQueue : BaseTaskQueue<UpdateServerTask>
{
    public UpdateServerTaskQueue(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }
}
