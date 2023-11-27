using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{


    public class MsgSender : MonoBehaviour
    {

        public void SendMsg( string msg_name )
        {
            MsgSystem.instance.SendMsg(msg_name, new object[] { });
        }


        public void SendMsg(string msg_name , string msg)
        {
            MsgSystem.instance.SendMsg(msg_name, new object[] { msg });
        }

    }
}

