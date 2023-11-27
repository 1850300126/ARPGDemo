using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace EasyUpdateDemoSDK
{

    public class CameraSystem : MonoBehaviour
    {
        public static CameraSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }

        public List<CameraItem> camera_items = new List<CameraItem>();

        public Camera current_camera;

        public void RegistCameraItem( CameraItem camera_item )
        {
            if (camera_items.Contains(camera_item) == false)
                camera_items.Add(camera_item);
        }


        public void RemoveCameraItem(CameraItem camera_item )
        {
            camera_items.Remove(camera_item);
        }


        [System.Serializable]
        public class Points
        {
            public string point_name = "";
            public Transform point;
        }

        public List<Points> points = new List<Points>();

        public Transform GetPoint(string point_name)
        {
            for (int i = 0; i < points.Count; i++)
                if (points[i].point_name.Equals(point_name) == true)
                    return points[i].point;

            return null;
        }


        [System.Serializable]
        public class CinemachineImpulseSources
        {
            public string impluse_name = "";
            public CinemachineImpulseSource impluse_source;
        }

        public List<CinemachineImpulseSources> impluse_sources = new List<CinemachineImpulseSources>();

        public CinemachineImpulseSource GetImpulseSource(string impluse_name)
        {
            for (int i = 0; i < impluse_sources.Count; i++)
                if (impluse_sources[i].impluse_name.Equals(impluse_name) == true)
                    return impluse_sources[i].impluse_source;

            return null;
        }


        public void ImpulseCamera(string impluse_name , Vector3 impluse_position)
        {
            CinemachineImpulseSource impulse_source = GetImpulseSource(impluse_name);

            if (impulse_source == null)
                return;

            impulse_source.transform.position = impluse_position;
            impulse_source.GenerateImpulse();
        }



    }
}

