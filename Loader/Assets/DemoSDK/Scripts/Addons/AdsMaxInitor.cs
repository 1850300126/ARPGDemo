using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class AdsMaxInitor : MonoBehaviour
    {
        public string sdk_key = "";

#if USE_MAX

        void Start()
        {
            MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
                MsgSystem.instance.SendBulletinMsg("ads_system_max_init_success", new object[] { } );
            };

            MaxSdk.SetSdkKey(sdk_key);

            MaxSdk.InitializeSdk();

        }

#endif

    }
}
