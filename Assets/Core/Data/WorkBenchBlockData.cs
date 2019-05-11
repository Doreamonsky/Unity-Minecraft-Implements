using UnityEngine;
namespace MC.Core
{
    public class WorkBenchBlockData : BlockData
    {
        public override void Interact(int height, int x, int y)
        {
            Debug.Log("WorkBenchBlockData");
        }
    }

}
