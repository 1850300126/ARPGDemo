using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EasyUpdateDemoSDK
{

    public class UIItem : BundleItem
    {

        public RectTransform rect_transform;
        public bool is_show_when_start = false;

        public virtual void OnBuildUI()
        {
            rect_transform = GetComponent<RectTransform>();

            UISystem.instance.RegistUI(this);
        }

        public virtual void OnStart()
        {
            if (is_show_when_start == true)
                ShowItem();
            else
                HideItem();
        }


        public virtual void ShowItem()
        {
            gameObject.SetActive(true);
        }


        public virtual void ShowItem(Dictionary<string, object> param)
        {
            ShowItem();
        }


        public virtual void HideItem()
        {
            gameObject.SetActive(false);
        }

        public virtual void HideItem(Dictionary<string, object> param)
        {
            HideItem();

            if (param != null && param.ContainsKey("hide_callback") == true)
                ((UnityAction)param["hide_callback"]).Invoke();
        }




        public virtual void DelayHideItem(float delay, Dictionary<string, object> param = null)
        {
            StartCoroutine(DelayHideItemCoroutine(delay, param));
        }

        IEnumerator DelayHideItemCoroutine(float delay_time, Dictionary<string, object> param = null)
        {
            yield return new WaitForSeconds(delay_time);

            if (param == null)
                HideItem();
            else
                HideItem(param);
        }

    }
}

