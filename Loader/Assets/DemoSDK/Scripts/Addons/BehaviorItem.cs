
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class BehaviorItem : MonoBehaviour
    {
        public virtual string GetItemName()
        {
            return "";
        }


        public virtual void BehaviorReport(string behavior_name, Dictionary<string, object> behavior_param)
        {
        }

    }
}
