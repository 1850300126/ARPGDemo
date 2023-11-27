using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EasyUpdateDemoSDK
{

    public class UIItemInfo : BundleItemInfo
    {


        [System.Serializable]
        public class Images
        {
            public string image_name = "";
            public Image image;
        }

        public List<Images> images = new List<Images>();

        public Image GetImage(string image_name)
        {
            for (int i = 0; i < images.Count; i++)
                if (images[i].image_name.Equals(image_name) == true)
                    return images[i].image;

            return null;
        }





        [System.Serializable]
        public class Texts
        {
            public string text_name = "";
            public TextMeshProUGUI text;
        }

        public List<Texts> texts = new List<Texts>();

        public TextMeshProUGUI GetText(string text_name)
        {
            for (int i = 0; i < texts.Count; i++)
                if (texts[i].text_name.Equals(text_name) == true)
                    return texts[i].text;

            return null;
        }






        [System.Serializable]
        public class Buttons
        {
            public string button_name = "";
            public Button button;
        }

        public List<Buttons> buttons = new List<Buttons>();

        public Button GetButton(string button_name)
        {
            for (int i = 0; i < buttons.Count; i++)
                if (buttons[i].button_name.Equals(button_name) == true)
                    return buttons[i].button;

            return null;
        }





    }
}
