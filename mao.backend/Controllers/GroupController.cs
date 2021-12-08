using mao.backend.Structures;

namespace mao.backend.Controllers;

public class GroupController
{
    #region Getters

    public static IEnumerable<int> GetAllGroupIds() => CoreController.GroupControls.Keys;
    public static IEnumerable<GroupControls> GetAllGroupControls() => CoreController.GroupControls.Values;
    public static GroupControls GetGroupControls(int groupId) => CoreController.GroupControls[groupId];
    public static IEnumerable<int> GetStreamIdsInGroup(int groupId) => GetGroupControls(groupId).StreamIds;
    public static IEnumerable<StreamControls> GetStreamControlsInGroup(int groupId) => GetStreamIdsInGroup(groupId).Select(StreamController.GetStreamControls).ToList();

    #endregion


    #region Controls
    
    public static void Kill(int groupId)
    {
        foreach (var streamId in GetStreamIdsInGroup(groupId))
        {
            StreamController.SetKill(streamId, true);
        }
        
        CoreController.GroupControls.Remove(groupId);
    }

    public static bool SetName(int groupId, string newValue)
    {
        if (!CoreController.GroupControls.ContainsKey(groupId)) return false;

        var controls = GetGroupControls(groupId);
        lock (controls)
        {
            controls.Name = newValue;
        }

        return true;
    }
    
    public static bool SetDeviceId(int groupId, int newValue)
    {
        if (!CoreController.GroupControls.ContainsKey(groupId)) return false;

        var controls = GetGroupControls(groupId);
        lock (controls)
        {
            controls.DeviceId = newValue;
        }

        return true;
    }

    #endregion
}