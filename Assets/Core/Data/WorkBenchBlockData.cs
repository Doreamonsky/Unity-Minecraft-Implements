using UnityEngine;
namespace MC.Core
{
    public class WorkBenchBlockData : BlockData
    {
        public override void RemoveBlock(WorldManager worldManager, int height, int x, int y)
        {
            Debug.Log("WorkBenchBlockData");
        }
    }

}
