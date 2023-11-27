using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class DrawSphere : MonoBehaviour
    {
        public Color color = Color.red;
        public float radius = 0.5f;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawWireSphere(transform.position, radius);
        }

    }
}

