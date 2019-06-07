using MC.Core.Interface;

namespace MC.Core
{
    [System.Serializable]
    public class InventoryStorage : IFileLoad, IFileSave
    {
        [System.NonSerialized]
        public Inventory inventory;

        public int count = -1;
        public int slotID = -1;

        public string inventoryName;

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

        public void OnLoad()
        {
            inventory = InventoryManager.Instance.GetInventoryByName(inventoryName);
        }

        public void OnSave()
        {
            inventoryName = inventory?.inventoryName;
        }
    }
}