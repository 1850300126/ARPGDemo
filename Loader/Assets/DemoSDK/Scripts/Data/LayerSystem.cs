using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class LayerSystem : MonoBehaviour
    {

        public static LayerSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }

        public LayerMask layer_default;

        [System.Serializable]
        public class LayerData
        {
            public string name = "";
            public LayerMask layer;
        }


        public List<LayerData> layers = new List<LayerData>();


        public LayerMask GetLayer( string layer_name )
        {
            for (int i = 0; i < layers.Count; i++)
                if (layers[i].name == layer_name)
                    return layers[i].layer;

            return layer_default;
        }

    }
}

