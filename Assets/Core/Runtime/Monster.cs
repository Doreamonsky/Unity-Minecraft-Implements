using UnityEngine;

namespace MC.Core
{
    public class Monster : MonoBehaviour
    {
        public float moveVelocity = 5;

        public float health = 100;

        [HideInInspector]
        public Player target;
        [HideInInspector]
        public CharacterController m_characterController;
        [HideInInspector]
        public Animator m_animator;

        public virtual void Start()
        {
            m_animator = GetComponent<Animator>();
            m_characterController = GetComponent<CharacterController>();
        }

        public virtual void Update()
        {
            if (target == null)
            {
                target = GameObject.FindObjectOfType<Player>();
                return;
            }

            var dir = Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up);

            var desireMove = moveVelocity * dir.normalized * Time.deltaTime;

            if (dir.magnitude > 15)
            {
                m_animator.SetFloat("Speed", 0.2f);
                m_characterController.Move(-Vector3.up * 9.8f * Time.deltaTime);
            }
            else
            {
                m_animator.SetFloat("Speed", 0f);
            }


            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(desireMove), Time.deltaTime * 2.5f);
        }

        public virtual void ApplyDamage(float damage)
        {
            health -= damage;

            PoolManager.CreateObject("Hurt Sound", transform.position, Vector3.zero);

            CheckDead();
        }

        public virtual void CheckDead()
        {
            if (health <= 0)
            {
                Destroy(gameObject);

                PoolManager.CreateObject("Explosion", transform.position, Quaternion.LookRotation(Vector3.up).eulerAngles);
            }
        }


    }

}
