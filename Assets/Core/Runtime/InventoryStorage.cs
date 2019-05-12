namespace MC.Core
{
    [System.Serializable]
    public class InventoryStorage
    {
        public int count = -1;
        public int slotID = -1;
        public Inventory inventory;

        public InventoryStorage()
        {

        }

        protected InventoryStorage(InventoryStorage other)
        {
            count = other.count;
            slotID = other.slotID;
            inventory = other.inventory;
        }

        public InventoryStorage Clone()
        {
            return new InventoryStorage(this);
        }
    }


}