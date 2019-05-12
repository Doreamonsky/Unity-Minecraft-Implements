using UnityEngine;

namespace MC.Core
{
    public struct InputData
    {
        public float x, y;
    }

    public class Player : MonoBehaviour
    {
        public System.Action<InputData> OnControllerInput;

        public float velocity = 10;

        public float jumpVelocity = 8.0f;

        public float gravity = -5f;

        public int blockID = 2;

        public bool isCreatorMode = false;

        private Vector3 moveDirection;

        private CharacterController m_CharacterContoller;

        private CameraController m_CameraController;

        private readonly float jumpingTime = 0;

        private void Start()
        {
            m_CharacterContoller = GetComponent<CharacterController>();
            m_CameraController = GetComponent<CameraController>();

            MouseLockModule.Instance.OnLock();

            OnControllerInput += data =>
            {
                var delta = m_CameraController.m_Camera.transform.forward * data.y + m_CameraController.m_Camera.transform.right * data.x;

                if (!isCreatorMode)
                {
                    delta = Vector3.ProjectOnPlane(delta, Vector3.up);
                }

                m_CharacterContoller.Move(delta * Time.deltaTime * velocity);
            };


        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            OnControllerInput?.Invoke(new InputData()
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            });

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CreateBlock();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                RemoveBlock();
            }

            if (!isCreatorMode)
            {
                m_CharacterContoller.Move(Vector3.up * gravity * Time.deltaTime);
            }
            //if (jumpingTime > 0)
            //{
            //    jumpingTime -= Time.deltaTime;
            //}
        }

        private void Jump()
        {
            Debug.Log("Jump");

            if (m_CharacterContoller.isGrounded)
            {
                m_CharacterContoller.Move(Vector3.up * jumpVelocity * Time.deltaTime);

            }
        }

        private void CreateBlock()
        {
            var cameraTrans = m_CameraController.m_Camera.transform;

            var isHit = Physics.Raycast(cameraTrans.position, cameraTrans.forward, out RaycastHit rayHit, 5, 1 << LayerMask.NameToLayer("Block"));

            if (isHit)
            {
                var chunckPoint = rayHit.point + rayHit.normal * 0.5f;

                chunckPoint = new Vector3(Mathf.FloorToInt(chunckPoint.x), Mathf.FloorToInt(chunckPoint.y), Mathf.FloorToInt(chunckPoint.z));

                WorldManager.Instance.CreateBlock((int)chunckPoint.y, (int)chunckPoint.x, (int)chunckPoint.z, blockID);
            }
        }

        private void RemoveBlock()
        {
            var cameraTrans = m_CameraController.m_Camera.transform;

            var isHit = Physics.Raycast(cameraTrans.position, cameraTrans.forward, out RaycastHit rayHit, 5, 1 << LayerMask.NameToLayer("Block"));

            if (isHit)
            {
                var chunckPoint = rayHit.point - rayHit.normal * 0.5f;

                chunckPoint = new Vector3(Mathf.FloorToInt(chunckPoint.x), Mathf.FloorToInt(chunckPoint.y), Mathf.FloorToInt(chunckPoint.z));

                WorldManager.Instance.InteractBlock((int)chunckPoint.y, (int)chunckPoint.x, (int)chunckPoint.z);
            }
        }
    }

}
