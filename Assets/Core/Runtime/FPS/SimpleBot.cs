using UnityEngine;
using UnityEngine.AI;

namespace MC.Core
{
    public class SimpleBot : Monster
    {
        public Transform destination;

        private NavMeshAgent meshAgent;

        private TPSAnimationController animationController;

        public override void Start()
        {
            //base.Start();
            meshAgent = GetComponent<NavMeshAgent>();
            animationController = GetComponentInChildren<TPSAnimationController>();
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
