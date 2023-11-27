using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUpdateDemoSDK
{

    public class TagSystem : MonoBehaviour
    {
        public static TagSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }



        public Dictionary<string, List<string>> tags = new Dictionary<string, List<string>>();


        public delegate void TagEvent(string tag_type, string tag);

        public Dictionary<string, Dictionary<string, List<TagEvent>>> tag_add_callback = new Dictionary<string, Dictionary<string, List<TagEvent>>>();
        public Dictionary<string, Dictionary<string, List<TagEvent>>> tag_remove_callback = new Dictionary<string, Dictionary<string, List<TagEvent>>>();



        public void TagAddRegist(string tag_type, string tag, TagEvent call_back)
        {
            if (tag_add_callback.ContainsKey(tag_type) == false)
                tag_add_callback.Add(tag_type, new Dictionary<string, List<TagEvent>>());

            if (tag_add_callback[tag_type].ContainsKey(tag) == false)
                tag_add_callback[tag_type].Add(tag, new List<TagEvent>());

            if (tag_add_callback[tag_type][tag].Contains(call_back) == false)
                tag_add_callback[tag_type][tag].Add(call_back);
        }

        public void TagAddUnRegist(string tag_type, string tag, TagEvent call_back)
        {
            if (tag_add_callback.ContainsKey(tag_type) == false)
                return;

            if (tag_add_callback[tag_type].ContainsKey(tag) == false)
                return;

            if (tag_add_callback[tag_type][tag].Contains(call_back) == true)
                tag_add_callback[tag_type][tag].Remove(call_back);
        }


        public void TagRemoveRegist(string tag_type, string tag, TagEvent call_back)
        {
            if (tag_remove_callback.ContainsKey(tag_type) == false)
                tag_remove_callback.Add(tag_type, new Dictionary<string, List<TagEvent>>());

            if (tag_remove_callback[tag_type].ContainsKey(tag) == false)
                tag_remove_callback[tag_type].Add(tag, new List<TagEvent>());

            if (tag_remove_callback[tag_type][tag].Contains(call_back) == false)
                tag_remove_callback[tag_type][tag].Add(call_back);
        }

        public void TagRemoveUnRegist(string tag_type, string tag, TagEvent call_back)
        {
            if (tag_remove_callback.ContainsKey(tag_type) == false)
                return;

            if (tag_remove_callback[tag_type].ContainsKey(tag) == false)
                return;

            if (tag_remove_callback[tag_type][tag].Contains(call_back) == true)
                tag_remove_callback[tag_type][tag].Remove(call_back);
        }


        public void OnTagAdd(string tag_type, string tag)
        {
            if (tag_add_callback.ContainsKey(tag_type) == false)
                return;

            if (tag_add_callback[tag_type].ContainsKey(tag) == false)
                return;

            for (int i = 0; i < tag_add_callback[tag_type][tag].Count; i++)
                tag_add_callback[tag_type][tag][i](tag_type, tag);
        }

        public void OnTagRemove(string tag_type, string tag)
        {
            if (tag_remove_callback.ContainsKey(tag_type) == false)
                return;

            if (tag_remove_callback[tag_type].ContainsKey(tag) == false)
                return;

            for (int i = 0; i < tag_remove_callback[tag_type][tag].Count; i++)
                tag_remove_callback[tag_type][tag][i](tag_type, tag);
        }

        public void TagAdd(string tag_type, string tag)
        {
            if (tags.ContainsKey(tag_type) == false)
                tags.Add(tag_type, new List<string>() { });

            if (tags[tag_type].Contains(tag) == false)
            {
                tags[tag_type].Add(tag);

                OnTagAdd(tag_type, tag);
            }
        }

        public void TagRemove(string tag_type, string tag)
        {
            if (tags.ContainsKey(tag_type) == false)
                return;

            if (tags[tag_type].Contains(tag) == false)
                return;

            tags[tag_type].Remove(tag);
            OnTagRemove(tag_type, tag);
        }


        public bool IsTagExist(string tag_type, string tag)
        {
            if (tags.ContainsKey(tag_type) == false)
                return false;

            if (tags[tag_type].Contains(tag) == false)
                return false;

            return true;
        }



        public static string tag_type_environment = "environment";
        public static string tag_type_emotion = "emotion";
        public static string tag_type_behavior = "behavior";
        public static string tag_type_action = "action";
        public static string tag_type_player_behavior = "player_behavior";
        public static string tag_type_player_action = "player_action";



        public static string tag_type_loader = "loader ";
        public static string tag_type_stage = "stage";
        public static string tag_type_module = "module";

    }
}

