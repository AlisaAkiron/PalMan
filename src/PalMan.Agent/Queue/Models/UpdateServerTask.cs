using PalMan.Agent.Entities;
using PalMan.Agent.Queue.Abstract;
using PalMan.Shared.Models.Requests;

namespace PalMan.Agent.Queue.Models;

public class UpdateServerTask : BaseTask
{
    public UpdateServerRequest RawRequest { get; set; } = null!;

    public PalWorldSettings UpdatedSettings { get; set; } = null!;
}
