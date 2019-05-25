using MC.Core.Interface;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "AttackableInventory", menuName = "AttackableInventory")]
    public class AttackableInventory : Inventory, IAttackable
    {
        public float Endurance, HPDamage;

        public GameObject weaponPrefab;

        public Vector3 slotPos, slotEulerAngle;

        private GameObject weaponModel;

        public float cropWoodSpeed = 1;

        public void Attack(Player attacker)
        {
            var animator = weaponModel.GetComponent<Animator>();

            if (animator)
            {
                animator.SetTrigger("Attack");
            }
        }


        public float GetEndurance()
        {
            return Endurance;
        }

        public float GetHPDamage()
        {
            return HPDamage;
        }

        public bool IsUseable()
        {
            return Endurance > 0;
        }

        public override void OnSelected(InventorySystem inventorySystem)
        {
            if (weaponModel == null)
            {
                weaponModel = Instantiate(weaponPrefab, inventorySystem.weaponSlot, true);
                weaponModel.transform.localPosition = slotPos;
                weaponModel.transform.localEulerAngles = slotEulerAngle;
            }

            weaponModel.SetActive(true);
        }

        public override void OnUnselected(InventorySystem inventorySystem)
        {
            weaponModel.SetActive(false);
        }

        public void UseEndurance(float usedEndurance)
        {
            Endurance -= usedEndurance;
        }

        public float GetCropWoodSpeed()
        {
            return cropWoodSpeed;
        }
    }
}
