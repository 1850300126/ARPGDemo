using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class AdsItem : MonoBehaviour
    {

        public virtual string GetItemName()
        {
            return "";
        }


        public virtual void ShowDebug( )
        {

        }

        public virtual bool IsInterstitalAdReady()
        {
            return false;
        }
        public virtual void ShowInterstitialAd(Action on_play_complete = null, Action on_play_failed = null, Action on_close = null)
        {
            
        }


        public virtual bool IsRewardAdReady()
        {
            return false;
        }

        public virtual void ShowRewardAd(Action on_play_complete = null, Action on_play_failed = null, Action on_close = null)
        {
        }

        public virtual bool IsOpenAdReady()
        {
            return false;
        }

        public virtual void ShowOpenAd(Action on_play_complete = null, Action on_play_failed = null, Action on_close = null)
        {
        }





    }
}
