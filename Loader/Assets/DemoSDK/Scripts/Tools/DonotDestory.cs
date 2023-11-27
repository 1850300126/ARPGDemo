using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class DonotDestory : MonoBehaviour
    {

        void Start()
        {
            DontDestroyOnLoad(gameObject);
        }


    }
}