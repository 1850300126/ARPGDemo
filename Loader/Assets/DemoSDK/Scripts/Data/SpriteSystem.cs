using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EasyUpdateDemoSDK
{

    public class SpriteSystem : MonoBehaviour
    {
        public static SpriteSystem instance;

        void Awake()
        {
            instance = this;
        }

        void OnDestroy()
        {
            instance = null;
        }

        [System.Serializable]
        public class SpriteFileInfo
        {
            public string name = "";
            public string file = "";
        }

        [System.Serializable]
        public class SpriteFileInfoList
        {
            public string type = "";
            public string path = "";
            public List<SpriteFileInfo> icons = new List<SpriteFileInfo>();
        }

        public List<SpriteFileInfoList> sprite_file_list = new List<SpriteFileInfoList>();


        private void Start()
        {
            MsgSystem.instance.RegistMsgAction(MsgString.build_all_config_finish, OnBuildConfigFinish);
        }

        public void OnBuildConfigFinish(object[] param)
        {
            sprite_file_list = ConfigSystem.instance.BuildConfig<SpriteFileInfoList>("icon_list");
        }


        public string GetIconFilePath(string sprite_type, string sprite_name )
        {
            for( int i = 0; i < sprite_file_list.Count; i ++ )
                if(sprite_file_list[i].type == sprite_type )
                    for( int j = 0; j < sprite_file_list[i].icons.Count; j ++ )
                        if (sprite_file_list[i].icons[j].name == sprite_name )
                            return sprite_file_list[i].path + "/"+sprite_file_list[i].icons[j].file;

            return "";
        }


        public Sprite LoadSprite(string sprite_type, string sprite_name)
        {
            string icon_path = GetIconFilePath(sprite_type, sprite_name);

            if (icon_path == "")
                return null;

            Sprite sprite = BundleInfoSystem.LoadAddressablesAsset<Sprite>(icon_path);

            SetSprite(sprite_type, sprite_name, sprite);

            return sprite;
        }



        [System.Serializable]
        public class SpriteItem
        {
            public string sprite_name = "";
            public Sprite sprite;
        }

        [System.Serializable]
        public class SpriteType
        {
            public string sprite_type = "";
            public List<SpriteItem> sprites = new List<SpriteItem>();
        }

        public List<SpriteType> sprites = new List<SpriteType>();




        public Sprite GetSprite(string sprite_type, string sprite_name)
        {
            for (int i = 0; i < sprites.Count; i++)
                if (sprites[i].sprite_type == sprite_type)
                {
                    for (int j = 0; j < sprites[i].sprites.Count; j++)
                        if (sprites[i].sprites[j].sprite_name == sprite_name)
                            return sprites[i].sprites[j].sprite;
                    break;
                }

            return LoadSprite(sprite_type, sprite_name);
        }



        public void SetSprite(string sprite_type, string sprite_name, Sprite sprite)
        {
            for (int i = 0; i < sprites.Count; i++)
                if (sprites[i].sprite_type == sprite_type)
                {
                    SetSprite(sprites[i], sprite_name, sprite);
                    return;
                }

            SpriteType item = new SpriteType();
            item.sprite_type = sprite_type;

            sprites.Add(item);

            SetSprite(item, sprite_name, sprite);
        }

        public void SetSprite(SpriteType sprite_type, string sprite_name, Sprite sprite)
        {
            for (int i = 0; i < sprite_type.sprites.Count; i++)
                if (sprite_type.sprites[i].sprite_name == sprite_name)
                {
                    sprite_type.sprites[i].sprite = sprite;
                    return;
                }

            SpriteItem item = new SpriteItem();
            item.sprite_name = sprite_name;
            item.sprite = sprite;

            sprite_type.sprites.Add(item);
        }


    }
}

