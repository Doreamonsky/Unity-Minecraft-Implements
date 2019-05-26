using UnityEngine;

namespace MC.Core.Interface
{
    //可放置物体
    public interface IPlaceable
    {
        void Place(WorldManager worldManager, Vector3 pos);
    }
}
