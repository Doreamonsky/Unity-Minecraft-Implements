using UnityEngine;

namespace MC.Core
{
    public enum NodeType
    {
        WayPoint,
        DefencePoint,
        AttackPoint
    }

    public class PlaceNode : MonoBehaviour
    {
        public NodeType nodeType = NodeType.WayPoint;
    }
}
