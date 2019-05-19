using UnityEngine;

namespace MC.Core
{
    public class CameraController : MonoBehaviour
    {
        //public static System.Action<InputData> OnControllerInput;

        //public Camera m_Camera;

        //private float x, y;

        //private bool isMobile = false;

        //private void Start()
        //{
        //    isMobile = Util.IsMobile();


        //    OnControllerInput += data =>
        //    {
        //        x += data.x * Time.deltaTime;
        //        y += data.y * Time.deltaTime;
        //    };
        //}

        private void Update()
        {
            //if (!isMobile)
            //{
            //    OnControllerInput?.Invoke(new InputData()
            //    {
            //        x = Input.GetAxis("Mouse X") * 50,
            //        y = Input.GetAxis("Mouse Y") * 50
            //    });
            //}

            //y = ClampAngle(y, -90, 90);

            //var rot = Quaternion.Euler(-y, x, 0);
            //m_Camera.transform.rotation = rot;
        }


        private float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
            {
                angle += 360;
            }

            if (angle > 360)
            {
                angle -= 360;
            }

            return Mathf.Clamp(angle, min, max);
        }
    }

}
