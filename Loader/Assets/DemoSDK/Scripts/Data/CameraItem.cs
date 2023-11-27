using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUpdateDemoSDK
{

    public class CameraItem : MonoBehaviour
    {
        public Camera camera_item;
        public Cinemachine.CinemachineVirtualCamera camera_virtual;

        void Start()
        {
            CameraSystem.instance.RegistCameraItem(this);
        }

    }
}

