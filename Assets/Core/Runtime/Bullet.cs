using UnityEngine;

namespace MC.Core
{
    public class Bullet : MonoBehaviour
    {
        public BulletData bulletData;

        private Vector3 dir;

        private Vector3 lastPos;


        //TODO Screen Ray 
        private void Start()
        {
            lastPos = transform.position;
            dir = transform.forward;

            Destroy(gameObject, 10);
        }



        private void FixedUpdate()
        {
            var nextPos = lastPos + dir * Time.fixedDeltaTime * bulletData.Velocity;

            var deltaDir = nextPos - lastPos;

            var isHit = Physics.Raycast(lastPos, dir, out RaycastHit rayHit, deltaDir.magnitude);

            if (isHit)
            {
                var monster = rayHit.collider.GetComponentInParent<Monster>();

                if (monster)
                {
                    monster.ApplyDamage(bulletData.Damage);
                }

                PoolManager.CreateObject(bulletData.hitEffect, rayHit.point + rayHit.normal * 0.1f, Quaternion.LookRotation(rayHit.normal).eulerAngles);
                Destroy(gameObject);
            }

            lastPos = nextPos;
            transform.position = nextPos;
        }
    }

}
