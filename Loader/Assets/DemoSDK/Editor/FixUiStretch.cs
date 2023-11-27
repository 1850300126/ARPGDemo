using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;

namespace EasyUpdateDemoSDK
{

    public class FixUiStretch : MonoBehaviour
    {
        [MenuItem("Tools/Fix UI Stretch")]
        static void fixUIStretch()
        {
            Object[] SelectionAsset = Selection.GetFiltered(typeof(GameObject), SelectionMode.Unfiltered);

            if (SelectionAsset.Length <= 0)
                return;

            for (int index = 0; index < SelectionAsset.Length; index++)
            {
                Debug.Log("fix ui stretch : " + SelectionAsset[index].name);

                GameObject Selected = (GameObject)SelectionAsset[index];

                RectTransform rectTransform = Selected.GetComponent<RectTransform>();

                if (rectTransform == null)
                    continue;

                if (Selected.transform.parent == null)
                    continue;

                RectTransform rectTransform_parent = Selected.transform.parent.GetComponent<RectTransform>();
                if (rectTransform_parent == null)
                    continue;

                //Debug.Log("RectTransform : " + rectTransform.rect);
                //Debug.Log("parent : " + rectTransform_parent.rect);

                float x = rectTransform.localPosition.x - rectTransform.rect.width * 0.5f + rectTransform_parent.rect.width * 0.5f;
                float y = rectTransform.localPosition.y - rectTransform.rect.height * 0.5f + rectTransform_parent.rect.height * 0.5f;
                float x2 = x + rectTransform.rect.width;
                float y2 = y + rectTransform.rect.height;

                //Debug.Log("result min : " + x + " , " + y + "   " + x / rectTransform_parent.rect.width + " , " + y / rectTransform_parent.rect.height);
                //Debug.Log("result max : " + x2 + " , " + y2 + "   " + x2 / rectTransform_parent.rect.width + " , " + y2 / rectTransform_parent.rect.height);


                rectTransform.anchorMin = new Vector2(x / rectTransform_parent.rect.width, y / rectTransform_parent.rect.height);
                rectTransform.anchorMax = new Vector2(x2 / rectTransform_parent.rect.width, y2 / rectTransform_parent.rect.height);

                rectTransform.anchoredPosition = Vector2.zero;
                rectTransform.sizeDelta = Vector2.zero;
            }
            




        }

    }


}

