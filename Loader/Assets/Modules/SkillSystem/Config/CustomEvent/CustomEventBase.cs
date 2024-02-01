using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public abstract class CustomEventBase : SerializedScriptableObject
{
    # region Method
    public abstract void EventStart();
    # endregion
}

