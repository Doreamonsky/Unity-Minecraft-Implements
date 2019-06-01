using MC.Core.Interface;
using UnityEngine;

namespace MC.Core
{
    [CreateAssetMenu(fileName = "AttackableInventory", menuName = "AttackableInventory")]
    public class AttackableInventory : Inventory, IAttackable, IDigBoost
    {
        private GameObject weaponModel;

        public float endurance = 100, hpDamage = 20;

        public float attackRange = 2;

        public float attackInterval = 0.2f;

        public GameObject weaponPrefab;

        public Vector3 slotPos, slotEulerAngle;

        public float digBoost = 1;

        [System.NonSerialized]
        private float lastAttackTime = 0;

        public void Attack(Player attacker)
        {
            if (Time.time - lastAttackTime > attackInterval)
            {
                lastAttackTime = Time.time;

                var animator = weaponModel.GetComponent<Animator>();

                if (animator)
                {
                    animator.SetTrigger("Attack");
                }

                var monsters = GameObject.FindObjectsOfType<Monster>();

                foreach (var monster in monsters)
                {
                    var dir = Vector3.ProjectOnPlane(monster.transform.position - attacker.transform.position, Vector3.up);

                    if (dir.magnitude < attackRange)
                    {
                        monster.ApplyDamage(hpDamage);
                    }
                }
            }

        }


        public float GetEndurance()
        {
            return endurance;
        }

        public float GetHPDamage()
        {
            return hpDamage;
        }

        public bool IsUseable()
        {
            return endurance > 0;
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
            if (weaponModel != null)
            {
                weaponModel.SetActive(false);
            }
        }

        public void UseEndurance(float usedEndurance)
        {
            endurance -= usedEndurance;
        }

        public float GetDigBoost()
        {
            return digBoost;
        }
    }
}
