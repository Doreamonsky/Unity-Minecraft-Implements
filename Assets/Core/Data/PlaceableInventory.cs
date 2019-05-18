using MC.Core.Interface;
using System;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName ="PlaceableInventory")]
    public class PlaceableInventory : Inventory, IPlaceable
    {
        public enum PlaceType
        {
            Block = 0
        }

        public PlaceType placeType = PlaceType.Block;

        public int blockID = 0;

        public void Place(Vector3 pos)
        {
            throw new NotImplementedException();
        }
    }
}
