using UnityEngine;

namespace MC.Core.Interface
{
    //可放置物体
    public interface IPlaceable
    {
        bool Place(WorldManager worldManager, Vector3Int pos, Vector3 dir);
    }
}
