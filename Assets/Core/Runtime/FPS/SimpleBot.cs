using UnityEngine;
using UnityEngine.AI;

namespace MC.Core
{
    public class SimpleBot : Monster
    {
        public Transform destination;

        public string currentWeapon;

        public CharacterData characterData;

        private NavMeshAgent meshAgent;

        private TPSAnimationController animationController;

        public override void Start()
        {
            //base.Start();

            //Init Bot Model
            var bot = Instantiate(characterData.character);
            bot.transform.SetParent(transform);

            bot.transform.localPosition = Vector3.zero;
            bot.transform.localRotation = Quaternion.identity;

            var slotData = characterData.weaponIKData.slotDataList.Find(val => val.weapon.name == currentWeapon);

            var weapon = Instantiate(slotData.weapon);
            weapon.transform.SetParent(bot.transform.Find("metarig/hips"));
            weapon.transform.localPosition = slotData.weaponPos;
            weapon.transform.localRotation = slotData.weaponRot;

            var leftHand = new GameObject("Left Hand");
            leftHand.transform.SetParent(weapon.transform);
            leftHand.transform.localPosition = slotData.leftHandPos;
            leftHand.transform.localRotation = slotData.leftHandRot;

            var rightHand = new GameObject("Right Hand");
            rightHand.transform.SetParent(weapon.transform);
            rightHand.transform.localPosition = slotData.rightHandPos;
            rightHand.transform.localRotation = slotData.rightHandRot;

            animationController = bot.AddComponent<TPSAnimationController>();
            animationController.weaponSlot = weapon.transform;

            meshAgent = gameObject.AddComponent<NavMeshAgent>();
            meshAgent.speed = 5;
            meshAgent.angularSpeed = 360;
            meshAgent.acceleration = 4;
        }

        public override void ApplyDamage(float damage)
        {
            base.ApplyDamage(damage);
        }
        public override void Update()
        {
            meshAgent.SetDestination(destination.position);
            animationController.velocity = meshAgent.velocity.magnitude / meshAgent.speed;
            //base.Update();
        }
    }
}
