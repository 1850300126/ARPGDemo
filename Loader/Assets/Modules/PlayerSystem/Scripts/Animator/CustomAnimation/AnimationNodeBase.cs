using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Custom.Animation
{

    public abstract class AnimationNodeBase
    {
        public int InputPort;

        public abstract void SetSpeed(float speed);
        public virtual void PushPool()
        {
            // PoolSystem.instance.PushObject(this);
        }
    }
}

