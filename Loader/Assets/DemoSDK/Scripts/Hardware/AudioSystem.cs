using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUpdateDemoSDK
{
    public enum AudioSourceType
    {
        effect,
        bgm,
    }

    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }


        public AudioSourceItem template_effect;
        public AudioSourceItem template_bgm;

        public List<AudioSourceItem> audio_sources = new List<AudioSourceItem>();

        public bool is_pause = false;

        public AudioSourceItem GetAudioSource(AudioSourceType audio_type)
        {
            for (int i = 0; i < audio_sources.Count; i++)
                if (audio_sources[i].audio_type == audio_type && audio_sources[i].is_in_use == false)
                    return audio_sources[i];

            AudioSourceItem audio_item = Instantiate(audio_type == AudioSourceType.effect ? template_effect : template_bgm, transform);
            audio_sources.Add(audio_item);

            return audio_item;
        }

        public void ReturnAudioItem( AudioSourceItem audio_item)
        {
            audio_item.is_in_use = false;

            audio_item.audio_source.Stop();
            audio_item.gameObject.SetActive(false);

            audio_item.transform.parent = transform;
        }


        AudioSourceItem PlayEffect(AudioClip clip, bool is_3d, bool is_loop)
        {
            AudioSourceItem audio_item = GetAudioSource(AudioSourceType.effect);

            audio_item.is_in_use = true;

            audio_item.gameObject.SetActive(true);

            audio_item.audio_source.spatialBlend = is_3d == true ? 1 : 0;
            audio_item.audio_source.loop = is_loop;

            audio_item.audio_source.clip = clip;

            audio_item.audio_source.Play();

            return audio_item;
        }



        public void PlayEffect(AudioClip clip, Vector3 position, float duration = 3.0f, bool is_3d = true)
        {
            AudioSourceItem audio_item = PlayEffect(clip, is_3d, false);

            audio_item.transform.position = position;

            StartCoroutine(PlayEffectFinish(audio_item,duration,null));
        }

        IEnumerator PlayEffectFinish(AudioSourceItem audio_item, float duration, Transform parent )
        {
            yield return new WaitForSeconds(duration);

            audio_item.is_in_use = false;
            audio_item.gameObject.SetActive(false);

            if(parent != null )
                audio_item.transform.parent = parent;
        }



        public void PlayerEffectWithTransform(AudioClip clip, Transform parent, float duration = 3.0f)
        {
            AudioSourceItem audio_item = PlayEffect(clip, true, false);

            audio_item.transform.parent = parent;
            audio_item.transform.localPosition = Vector3.zero;

            StartCoroutine(PlayEffectFinish(audio_item, duration, transform));
        }


        public void PlayerLoopEffectWithTramsform(AudioClip clip, Transform parent)
        {
            AudioSourceItem audio_item = PlayEffect(clip, true, true);

            audio_item.transform.parent = parent;
            audio_item.transform.localPosition = Vector3.zero;

        }

        public void StopEffect(AudioClip clip)
        {
            for (int i = 0; i < audio_sources.Count; i++)
            {
                if (audio_sources[i].audio_source.clip == clip)
                {
                    audio_sources[i].audio_source.Stop();

                    audio_sources[i].is_in_use = false;
                    audio_sources[i].gameObject.SetActive(false);
                    audio_sources[i].transform.parent = transform;
                }
            }
        }




        public AudioSourceItem PlayBGM(AudioClip clip)
        {
            AudioSourceItem audio_item = GetAudioSource(AudioSourceType.bgm);
            audio_item.is_in_use = true;

            audio_item.gameObject.SetActive(true);
            audio_item.audio_source.clip = clip;
            audio_item.audio_source.Play();

            return audio_item;

        }


        public void StopAllBGM()
        {
            for (int i = 0; i < audio_sources.Count; i++)
                if (audio_sources[i].audio_type == AudioSourceType.bgm)
                    audio_sources[i].audio_source.Stop();
        }



        public void SetPause(bool pause)
        {
            is_pause = pause;

            for (int i = 0; i < audio_sources.Count; i++)
                audio_sources[i].audio_source.mute = is_pause;
        }

    }
}
