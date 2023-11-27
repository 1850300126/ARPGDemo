using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace EasyUpdateDemoSDK
{



    public class StageItem : BundleItem
    {
        public override void OnLoaded( )
        {
            if(StageSystem.instance != null )
                StageSystem.instance.current_stage = this;
        }


    }
}


