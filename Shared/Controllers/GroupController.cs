using System.Collections.Generic;
using System.Linq;
using mao_mudblazor_server.Shared.Structures;

namespace mao_mudblazor_server.Shared.Controllers;

public class GroupController
{
    #region Getters

    public static IEnumerable<int> GetAllGroupIds() => CoreController.GroupControls.Keys;
    public static IEnumerable<NewGroupControls> GetAllGroupControls() => CoreController.GroupControls.Values;
    public static NewGroupControls GetGroupControls(int groupId) => CoreController.GroupControls[groupId];
    public static IEnumerable<int> GetStreamIdsInGroup(int groupId) => GetGroupControls(groupId).StreamIds;
    public static IEnumerable<NewStreamControls> GetStreamControlsInGroup(int groupId) => GetStreamIdsInGroup(groupId).Select(StreamController.GetStreamControls).ToList();

    #endregion


    #region Controls

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