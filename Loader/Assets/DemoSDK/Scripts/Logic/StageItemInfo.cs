using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EasyUpdateDemoSDK
{


    public class StageItemInfo : BundleItemInfo
    {
        public Camera stage_camera;

        public void Awake()
        {
            OnLoaded();
        }



        [System.Serializable]
        public class StageParticle
        {
            public string point_name = "";
            public ParticleSystem particle;
        }

        public List<StageParticle> stage_particles = new List<StageParticle>();

        public ParticleSystem GetParticle(string point_name)
        {
            for (int i = 0; i < stage_particles.Count; i++)
                if (stage_particles[i].point_name.Equals(point_name) == true)
                    return stage_particles[i].particle;

            return null;
        }



        [System.Serializable]
        public class StageTexture2D
        {
            public string point_name = "";
            public Texture2D texture;
        }

        public List<StageTexture2D> stage_textures = new List<StageTexture2D>();

        public Texture2D GetTexture2D(string point_name)
        {
            for (int i = 0; i < stage_textures.Count; i++)
                if (stage_textures[i].point_name.Equals(point_name) == true)
                    return stage_textures[i].texture;

            return null;
        }




    }
}
