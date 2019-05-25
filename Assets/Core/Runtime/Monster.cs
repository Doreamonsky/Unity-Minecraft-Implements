using UnityEngine;

namespace MC.Core
{
    public class Monster : MonoBehaviour
    {
        public float moveVelocity = 5;

        public float health = 100;

        private Player target;

        private CharacterController m_characterController;

        private Animator m_animator;

        private void Start()
        {
            m_animator = GetComponent<Animator>();
            m_characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {
            if (target == null)
            {
                target = GameObject.FindObjectOfType<Player>();
                return;
            }

            var dir = target.transform.position - transform.position;

            var desireMove = Vector3.ProjectOnPlane(moveVelocity * dir.normalized * Time.deltaTime, Vector3.up);

            if (dir.magnitude > 5)
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

        public void ApplyDamage(float damage)
        {
            health -= damage;

            CheckDead();
        }

        private void CheckDead()
        {
            if (health <= 0)
            {
                Destroy(gameObject);

                PoolManager.CreateObject("Explosion", transform.position, Quaternion.LookRotation(Vector3.up).eulerAngles);
            }
        }
    }

}
