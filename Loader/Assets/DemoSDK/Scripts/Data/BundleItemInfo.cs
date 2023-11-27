using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class BundleItemInfo : MonoBehaviour
    {
        public string item_class = "";
        public BundleItem bundle_item;


        public virtual void OnLoaded()
        {
            if (item_class.Equals("") == true)
                return;

            Type module_type = StringUtility.FindType(item_class);

            if (module_type == null)
            {
                Debug.Log("BundleItemInfo : " + gameObject.name + "  " + item_class + " not found !");

                return;
            }

            gameObject.AddComponent(module_type);

            bundle_item = gameObject.GetComponent<BundleItem>();

            if (bundle_item == null)
            {
                Debug.Log("BundleItem : " + gameObject.name + " not found !");
                return;
            }

            bundle_item.item_info = this;
            bundle_item.OnLoaded();
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
        public Transform SetPoint(string point_name,Transform point)
        {
            for (int i = 0; i < points.Count; i++)
                if (points[i].point_name.Equals(point_name) == true)
                    return points[i].point = point ;

            return null;
        }


        [System.Serializable]
        public class Values
        {
            public string value_name = "";
            public float value;
        }

        public List<Values> values = new List<Values>();

        public float GetValue(string value_name)
        {
            for (int i = 0; i < values.Count; i++)
                if (values[i].value_name.Equals(value_name) == true)
                    return values[i].value;

            return 0;
        }

    }
}

