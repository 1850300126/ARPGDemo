using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class NewPlayableAsset : PlayableAsset
{   
    public NewPlayableBehaviour newPlayableBehaviour = new NewPlayableBehaviour();

    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var scriptPlayable = ScriptPlayable<NewPlayableBehaviour>.Create(graph, newPlayableBehaviour);

        return scriptPlayable;
    }
}
