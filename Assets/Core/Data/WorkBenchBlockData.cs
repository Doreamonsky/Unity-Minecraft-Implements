using UnityEngine;
namespace MC.Core
{
    public class WorkBenchBlockData : BlockData
    {
        public override void RemoveBlock(int height, int x, int y)
        {
            Debug.Log("WorkBenchBlockData");
        }
    }

}
