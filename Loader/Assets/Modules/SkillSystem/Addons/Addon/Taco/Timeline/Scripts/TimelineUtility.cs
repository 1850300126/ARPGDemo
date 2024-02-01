using System;
using UnityEngine;

namespace Taco.Timeline 
{
    public enum ExtraPolationMode
    {
        None = 0,
        Hold = 1,
    }

    [Flags]
    public enum ClipCapabilities
    {
        None = 0,
        Resizable = 0x1,
        Mixable = 0x2,
        ClipInable = 0x4,
    }

    public delegate void Evaluate(float deltaTime);

    public static class TimelineUtility
    {
        public static int FrameRate = 60;
        public static float MinEvaluateDeltaTime
        {
            get
            {
                if (Application.isPlaying)
                    return UnityEngine.Time.deltaTime;
                return 1f / FrameRate;
            }
        }

        public static void Lerp(float targetTime, float deltaTime, Evaluate evaluateSplitDeltaTime, ref float lastTime)
        {
            int direction = deltaTime > 0 ? 1 : -1; //����or����
            if (Mathf.Abs(deltaTime) > MinEvaluateDeltaTime)
            {
                while (lastTime != targetTime)
                {
                    float splitDeltaTime = direction * MinEvaluateDeltaTime;
                    if (direction == 1)
                    {
                        splitDeltaTime = Mathf.Min(splitDeltaTime, targetTime - lastTime);
                    }
                    else
                    {
                        splitDeltaTime = Mathf.Max(splitDeltaTime, targetTime - lastTime);
                    }
                    evaluateSplitDeltaTime(splitDeltaTime);
                    lastTime += splitDeltaTime;
                }
            }
            else
            {
                evaluateSplitDeltaTime(deltaTime);
                lastTime += deltaTime;
            }
        }
    }
}