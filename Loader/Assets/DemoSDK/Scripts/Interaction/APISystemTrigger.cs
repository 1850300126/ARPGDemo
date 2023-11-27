using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUpdateDemoSDK
{


    public class APISystemTrigger : MonoBehaviour
    {

        public string api_name = "";
        public string api_function = "";


        public void OnTriggerAPI( )
        {
            APISystem.instance.CallAPI(api_name, api_function);
        }


    }
}
