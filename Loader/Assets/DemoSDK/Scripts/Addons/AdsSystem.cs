using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using DG.Tweening;
using UnityEngine.UI;
using System;

namespace EasyUpdateDemoSDK
{


    public class AdsSystem : MonoBehaviour
    {
        public static AdsSystem instance;

        void Awake()
        {
            instance = this;

            RegistAPI();
        }

        void OnDestroy()
        {
            instance = null;
        }

        public List<AdsItem> ads_items = new List<AdsItem>();


        public void RegistAPI( )
        {
            APISystem.instance.RegistAPI(APIString.ads_system, OnAPIFunction);
        }



        public object OnAPIFunction( string function, object[] param)
        {
            Debug.Log("OnAPIFunction : ads_system " + function);

            if (function == APIString.show_debug)
                ShowDebug();

            return null;
        }





        public void RegistItem( AdsItem item )
        {
            ads_items.Add( item );
        }



        public void ShowDebug(string item_name = "max")
        {
            for (int i = 0; i < ads_items.Count; i++)
                if ( ads_items[i].GetItemName() == item_name)
                    ads_items[i].ShowDebug();
        }


        public bool IsInterstitalAdReady( string item_name = "max")
        {
            for (int i = 0; i < ads_items.Count; i++)
                if (ads_items[i].IsInterstitalAdReady() == true && ads_items[i].GetItemName() == item_name )
                    return true;

            return false;
        }

        public void ShowInterstitialAd(Action on_play_complete = null, Action on_play_failed = null, Action on_close = null, string item_name = "max")
        {
            for (int i = 0; i < ads_items.Count; i++)
                if (ads_items[i].IsInterstitalAdReady() == true && ads_items[i].GetItemName() == item_name)
                {
                    ads_items[i].ShowInterstitialAd(on_play_complete, on_play_failed, on_close);
                    
                    return;
                }
        }



        public bool IsRewardAdReady(string item_name = "max")
        {
            for (int i = 0; i < ads_items.Count; i++)
                if (ads_items[i].IsRewardAdReady() == true && ads_items[i].GetItemName() == item_name)
                    return true;

            return false;
        }


        public void ShowRewardAd(Action on_play_complete = null, Action on_play_failed = null, Action on_close = null, string item_name = "max")
        {
            for (int i = 0; i < ads_items.Count; i++)
                if (ads_items[i].IsRewardAdReady() == true && ads_items[i].GetItemName() == item_name)
                {
                    ads_items[i].ShowRewardAd(on_play_complete, on_play_failed, on_close);

                    return;
                }
        }




        public bool IsOpenAdReady(string item_name = "max")
        {
            for (int i = 0; i < ads_items.Count; i++)
                if (ads_items[i].IsOpenAdReady() == true && ads_items[i].GetItemName() == item_name)
                    return true;

            return false;
        }


        public void ShowOpenAd(Action on_play_complete = null, Action on_play_failed = null, Action on_close = null, string item_name = "max")
        {
            for (int i = 0; i < ads_items.Count; i++)
                if (ads_items[i].IsOpenAdReady() == true && ads_items[i].GetItemName() == item_name)
                {
                    ads_items[i].ShowOpenAd(on_play_complete, on_play_failed, on_close);

                    return;
                }
        }





    }
}
