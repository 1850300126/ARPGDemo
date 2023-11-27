using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class ColorSystem : MonoBehaviour
    {
        public static ColorSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }



        [System.Serializable]
        public class ColorItem
        {
            public string color_name = "";
            public Color color = Color.white;
        }

        [System.Serializable]
        public class ColorType
        {
            public string color_type = "";
            public List<ColorItem> colors = new List<ColorItem>();
        }

        public List<ColorType> colors = new List<ColorType>();


        public Color GetColor( string color_type , string color_name )
        {
            for( int i = 0; i < colors.Count; i ++ ) 
                if(colors[i].color_type == color_type )
                {
                    for (int j = 0; j < colors[i].colors.Count; j++)
                        if (colors[i].colors[j].color_name == color_name)
                            return colors[i].colors[j].color;
                    break;
                }

            return Color.white;
        }



        public void SetColor(string color_type, string color_name , Color color )
        {
            for (int i = 0; i < colors.Count; i++)
                if (colors[i].color_type == color_type)
                {
                    SetColor(colors[i] , color_name, color);
                    return;
                }

            ColorType item = new ColorType();
            item.color_type = color_type;

            colors.Add(item);

            SetColor(item, color_name, color);
        }

        public void SetColor( ColorType color_type, string color_name, Color color)
        {
            for (int i = 0; i < color_type.colors.Count; i++)
                if (color_type.colors[i].color_name == color_name)
                {
                    color_type.colors[i].color = color;
                    return;
                }

            ColorItem item = new ColorItem();
            item.color_name = color_name;
            item.color = color;

            color_type.colors.Add(item);
        }

    }
}

