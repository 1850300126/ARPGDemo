using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;

namespace EasyUpdateDemoSDK
{

    public class CommonUIInfo : CommonInfo
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

        [System.Serializable]
        public class Inputs
        {
            public string input_name = "";
            public TMP_InputField input;
        }

        public List<Inputs> inputs = new List<Inputs>();

        public TMP_InputField GetInput(string input_name)
        {
            for (int i = 0; i < inputs.Count; i++)
                if (inputs[i].input_name.Equals(input_name) == true)
                    return inputs[i].input;

            return null;
        }



    }
}
