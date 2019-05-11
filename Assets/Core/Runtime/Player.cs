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

        private Vector3 moveDirection;

        private CharacterController m_CharacterContoller;

        private CameraController m_CameraController;

        private void Start()
        {
            m_CharacterContoller = GetComponent<CharacterController>();
            m_CameraController = GetComponent<CameraController>();

            MouseLockModule.Instance.OnLock();

            OnControllerInput += data =>
            {
                if (m_CharacterContoller.isGrounded)
                {
                    var delta = m_CameraController.m_Camera.transform.forward * data.y + m_CameraController.m_Camera.transform.right * data.x;

                    delta = Vector3.ProjectOnPlane(delta, Vector3.up);

                    m_CharacterContoller.Move(delta * Time.deltaTime * velocity);
                }
            };


        }

        private void Update()
        {
            OnControllerInput?.Invoke(new InputData()
            {
                x = Input.GetAxis("Horizontal"),
                y = Input.GetAxis("Vertical")
            });

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                CreateBlock();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                RemoveBlock();
            }

            m_CharacterContoller.Move(Vector3.up * gravity * Time.deltaTime);
        }

        private void Jump()
        {
            if (m_CharacterContoller.isGrounded)
            {
                m_CharacterContoller.Move(Vector3.up * jumpVelocity * Time.deltaTime);
            }
        }

        private void CreateBlock()
        {
            var cameraTrans = m_CameraController.m_Camera.transform;

            var isHit = Physics.Raycast(cameraTrans.position, cameraTrans.forward, out RaycastHit rayHit, 100, 1 <<LayerMask.NameToLayer("Block"));

            if (isHit)
            {
                var chunckPoint = rayHit.point + rayHit.normal * 0.5f;

                chunckPoint = new Vector3(Mathf.FloorToInt(chunckPoint.x), Mathf.FloorToInt(chunckPoint.y), Mathf.FloorToInt(chunckPoint.z));

                WorldManager.Instance.CreateBlock((int)chunckPoint.y, (int)chunckPoint.x, (int)chunckPoint.z, 2);
            }
        }

        private void RemoveBlock()
        {
            var cameraTrans = m_CameraController.m_Camera.transform;

            var isHit = Physics.Raycast(cameraTrans.position, cameraTrans.forward, out RaycastHit rayHit, 100);

            if (isHit)
            {
                var chunckPoint = rayHit.point - rayHit.normal * 0.5f;

                chunckPoint = new Vector3(Mathf.FloorToInt(chunckPoint.x), Mathf.FloorToInt(chunckPoint.y), Mathf.FloorToInt(chunckPoint.z));

                WorldManager.Instance.RemoveBlock((int)chunckPoint.y, (int)chunckPoint.x, (int)chunckPoint.z);
            }
        }
    }

}
