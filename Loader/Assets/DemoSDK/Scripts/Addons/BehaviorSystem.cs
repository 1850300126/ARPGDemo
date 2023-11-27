using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUpdateDemoSDK
{


    public class BehaviorSystem : MonoBehaviour
    {
        public static BehaviorSystem instance;

        void Awake()
        {
            instance = this;

            RegistAPI();
        }

        void OnDestroy()
        {
            instance = null;
        }

        public List<BehaviorItem> behavior_items = new List<BehaviorItem>();

        public void RegistAPI()
        {
            APISystem.instance.RegistAPI(APIString.behavior_system, OnAPIFunction);
        }


        public object OnAPIFunction(string function, object[] param)
        {
            Debug.Log("OnAPIFunction : behavior_system " + function);

            if (function == APIString.behavior_report )
                BehaviorReport((string)param[0], (Dictionary<string, object>)param[1] , param.Length>=3? (string)param[2]:"" );

            return null;
        }




        public void RegistItem(BehaviorItem item)
        {
            behavior_items.Add(item);
        }


        public virtual void BehaviorReport(string behavior_name, Dictionary<string, object> behavior_param, string item_name = "")
        {
            for (int i = 0; i < behavior_items.Count; i++)
                if (item_name == ""  || behavior_items[i].GetItemName() == item_name)
                    behavior_items[i].BehaviorReport(behavior_name, behavior_param);

        }

    }
}
