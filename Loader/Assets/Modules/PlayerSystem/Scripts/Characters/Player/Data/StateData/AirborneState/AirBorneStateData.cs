using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AirBorneStateData
{
    [field: SerializeField] public PlayerJumpData JumpData { get; private set; }
}
