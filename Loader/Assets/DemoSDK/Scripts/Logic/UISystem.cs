using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace EasyUpdateDemoSDK
{

    public class UISystem : MonoBehaviour
    {
        public static UISystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        public List<UIItem> ui_items = new List<UIItem>();

        

        public void RegistUI(UIItem item)
        {
            if (ui_items.Contains(item) == false)
                ui_items.Add(item);
        }



        public void LoadUI(string module_name)
        {
            LoadUI(module_name, () => { });
        }


        public void LoadUI(string item_name, UnityAction on_load_finish)
        {
            BundleInfoSystem.BundleInfoItem data = BundleInfoSystem.instance.GetBundleInfoItem(item_name, "UI");

            if (data == null)
                return;

            LoadUI(data, on_load_finish);
        }


        public void LoadUI(BundleInfoSystem.BundleInfoItem item_data)
        {
            LoadUI(item_data, () => { });
        }


        public void LoadUI(BundleInfoSystem.BundleInfoItem item_data, UnityAction on_load_finish)
        {
            BundleInfoSystem.LoadBundleItem(item_data, ui_items, transform);

            on_load_finish?.Invoke();
        }






        public UIItem GetUI(string ui_name)
        {
            for (int i = 0; i < ui_items.Count; i++)
            {
                if (ui_items[i].name == ui_name)
                    return ui_items[i];
            }

            return null;
        }

        public bool IsInputOK(TMP_InputField input, string title , string panel_name )
        {
            if (input.text == null || input.text.Trim() == "")
            {
                GetUI(panel_name).ShowItem(new Dictionary<string, object>() { { "text_title", title }, { "btn_ok", "show" } });

                return false;
            }

            return true;
        }


        public void SwitchUI(string ui_from, string ui_to, Dictionary<string, object> param)
        {
            SwitchUI(ui_from, ui_to, param, 0.3f, -Screen.width * 0.5f, Screen.width * 1.5f);
        }


        public void SwitchUIBack(string ui_from, string ui_to, Dictionary<string, object> param)
        {
            SwitchUI(ui_from, ui_to, param, 0.3f, Screen.width * 1.5f, -Screen.width * 0.5f);
        }


        public void SwitchUI(string ui_from, string ui_to, Dictionary<string, object> param, float duration, float from_x_to, float to_x_from)
        {
            UIItem from = GetUI(ui_from);
            UIItem to = GetUI(ui_to);

            if (from == null || to == null)
                return;

            DOTween.Kill(from);
            DOTween.Kill(to);

            from.rect_transform.DOMoveX(from_x_to, duration).OnComplete(() => from.HideItem());

            to.ShowItem(param);
            to.rect_transform.position = new Vector3(to_x_from, Screen.height * 0.5f, 0);
            to.rect_transform.DOMoveX(Screen.width * 0.5f, duration);
        }





        public bool IsPointerOverUI(Vector2 mouse_position , List<string> ignore_list )
        {
            PointerEventData event_data = new PointerEventData(EventSystem.current);
            event_data.position = mouse_position;

            List<RaycastResult> raycast_results = new List<RaycastResult>();

            EventSystem.current.RaycastAll(event_data, raycast_results);

            for( int i = raycast_results.Count - 1; i >= 0; i -- )
                if(ignore_list.Contains(raycast_results[i].gameObject.name) == true )
                    raycast_results.RemoveAt(i);

            return raycast_results.Count > 0;
        }



    }
}
