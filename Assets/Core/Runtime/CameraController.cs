using UnityEngine;

namespace MC.Core
{
    public class CameraController : MonoBehaviour
    {
        public System.Action<InputData> OnControllerInput;

        public Camera m_Camera;

        private float x, y;

        private void Start()
        {
            OnControllerInput += data =>
            {
                x += data.x * Time.deltaTime;
                y += data.y * Time.deltaTime;
            };
        }

        private void Update()
        {
            OnControllerInput?.Invoke(new InputData()
            {
                x = Input.GetAxis("Mouse X") * 50,
                y = Input.GetAxis("Mouse Y") * 50
            });

            var rot = Quaternion.Euler(-y, x, 0);
            m_Camera.transform.rotation = rot;
        }
    }

}
