using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "RecipeData", menuName = "RecipeData")]
    public class RecipeData : ScriptableObject
    {
        public Inventory[] Recipe = new Inventory[9];

        public Inventory CraftedInventory;
    }
}
