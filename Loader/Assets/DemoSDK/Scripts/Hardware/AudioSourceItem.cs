using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class AudioSourceItem : MonoBehaviour
    {
        public AudioSourceType audio_type = AudioSourceType.effect;
        public AudioSource audio_source;
        public bool is_in_use = false;

    }
}

