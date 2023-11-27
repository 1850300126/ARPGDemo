using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EasyUpdateDemoSDK
{

    public class MultiClickButton : MonoBehaviour
    {

        public float click_count = 10;
        public float click_cd = 0.2f;

        public float last_click_time = 0;
        public float current_click_count = 0;

        public UnityEvent on_trigger_event;

        public void OnClickBtn()
        {
            if ((Time.time - last_click_time) <= click_cd)
            {
                current_click_count++;

                if (current_click_count >= click_count)
                    OnTriggerEvent();
            }
            else
                current_click_count = 0;

            last_click_time = Time.time;

        }


        public void OnTriggerEvent()
        {
            current_click_count = 0;

            on_trigger_event?.Invoke();
        }

    }
}

