using UnityEngine;

namespace MC.Core.Interface
{
    //可破坏的
    public interface IDigable
    {
        float DigTime();

        AudioClip DigSound();

        void DropInventory(Vector3 pos);
    }
}
