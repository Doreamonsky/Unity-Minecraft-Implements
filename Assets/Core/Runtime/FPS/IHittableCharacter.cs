using UnityEngine;

namespace MC.Core
{
    public interface IHittableCharacter
    {
        Vector3 GetSpotPoint();
        Transform GetAimTransform();
    }
}
