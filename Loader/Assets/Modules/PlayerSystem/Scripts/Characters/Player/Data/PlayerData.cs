using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "MachineData/Characters/Player")]
public class PlayerSO : ScriptableObject
{

    [field: SerializeField] public AirBorneStateData air_borne_data { get; private set; }
    [field: SerializeField] public GroundedStateData grounded_data { get; private set; }
    [field: SerializeField] public PlayerSelfData self_data { get; private set; }
}
// [Serializable]
// public class PlayerData
// {

//     [field: SerializeField] public AirBorneStateData air_borne_data { get; private set; }
//     [field: SerializeField] public GroundedStateData grounded_data { get; private set; }
//     [field: SerializeField] public PlayerSelfData self_data { get; private set; }
// }

