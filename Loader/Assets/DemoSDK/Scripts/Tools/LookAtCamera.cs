using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{
    public class LookAtCamera : MonoBehaviour
    {

        public bool is_forward_z = true;
        public bool is_only_y = false;
        public bool is_use_camera_system = false;

        Vector3 look_at_point = Vector3.zero;

        private void Update()
        {
            Camera camera = Camera.main;

            if (is_use_camera_system == true )
            {
                if (CameraSystem.instance == null || CameraSystem.instance.current_camera == null )
                    return;

                camera = CameraSystem.instance.current_camera;
            }
            
            if (camera == null)
                return;

            look_at_point = camera.transform.position;

            if (is_forward_z == false)
                look_at_point = transform.position + (transform.position - camera.transform.position);

            if (is_only_y == true)
                look_at_point.y = transform.position.y;

            transform.LookAt(look_at_point);
        }





    }
}

