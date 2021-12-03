using System.Collections.Generic;

namespace mao_mudblazor_server.Shared.Structures;

public class NewGroupControls
{
    public int GroupId { get; set; } = -1;
    public string Name { get; set; } = "New";
    public int DeviceId { get; set; } = -1;
    public HashSet<int> StreamIds { get; set; } = new ();


    public bool AddNewStreamId(int streamId) => StreamIds.Add(streamId);
}