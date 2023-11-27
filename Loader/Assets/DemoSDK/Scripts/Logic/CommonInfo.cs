using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class CommonInfo : MonoBehaviour
    {


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
        public class Strings
        {
            public string string_name = "";
            public string string_value;
        }

        public List<Strings> strings = new List<Strings>();

        public string GetString(string string_name)
        {
            for (int i = 0; i < strings.Count; i++)
                if (strings[i].string_name.Equals(string_name) == true)
                    return strings[i].string_value;

            return "";
        }


        [System.Serializable]
        public class Curves
        {
            public string curve_name = "";
            public AnimationCurve curve;
        }

        public List<Curves> curves = new List<Curves>();

        public AnimationCurve GetCurve(string curve_name)
        {
            for (int i = 0; i < curves.Count; i++)
                if (curves[i].curve_name.Equals(curve_name) == true)
                    return curves[i].curve;

            return null;
        }




        [System.Serializable]
        public class Configs
        {
            public string config_name = "";
            public TextAsset config_file;
        }

        public List<Configs> configs = new List<Configs>();

        public TextAsset GetConfigs(string config_name)
        {
            for (int i = 0; i < configs.Count; i++)
                if (configs[i].config_name.Equals(config_name) == true)
                    return configs[i].config_file;

            return null;
        }


    }
}
