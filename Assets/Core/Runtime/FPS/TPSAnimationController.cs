using UnityEngine;


namespace MC.Core
{
    public class TPSAnimationController : MonoBehaviour
    {
        public Transform weaponSlot;

        private Transform leftHand, rightHand;

        public float leftHandWeight = 1, rightHandWeight = 1;

        public bool isAimingTarget = false;

        public Transform target;

        public float velocity;

        private Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();

            leftHand = weaponSlot.Find("Left Hand");
            rightHand = weaponSlot.Find("Right Hand");
        }

        private void Update()
        {
            if (isAimingTarget)
            {
                if (target == null)
                {
                    isAimingTarget = false;
                    return;
                }

                var dir = target.position - weaponSlot.position;

                weaponSlot.rotation = Quaternion.LookRotation(dir);

                var characterRot = Quaternion.LookRotation(Vector3.ProjectOnPlane(dir, Vector3.up));

                transform.rotation = Quaternion.Lerp(transform.rotation, characterRot, Time.deltaTime * 5);
            }

            animator.SetFloat("velocity", velocity);
        }

        private void OnAnimatorIK(int layerIndex)
        {
            animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.transform.position);
            animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.transform.rotation);
            animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftHandWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftHandWeight);

            animator.SetIKPosition(AvatarIKGoal.RightHand, rightHand.transform.position);
            animator.SetIKRotation(AvatarIKGoal.RightHand, rightHand.transform.rotation);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightHandWeight);
            animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightHandWeight);
        }
    }

}

