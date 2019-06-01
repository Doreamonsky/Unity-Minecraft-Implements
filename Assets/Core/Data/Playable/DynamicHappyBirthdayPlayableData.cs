#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Playables;
namespace MC.Core
{
    public class DynamicHappyBirthdayPlayable : PlayableBehaviour
    {
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                return;
            }
#endif
            new GameObject("BirthDay Block", typeof(DynamicBirthDayBlock));
        }
    }

    [System.Serializable]
    public class DynamicHappyBirthdayPlayableData : PlayableAsset
    {
        // Factory method that generates a playable based on this asset
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            return ScriptPlayable<DynamicHappyBirthdayPlayable>.Create(graph);
        }
    }

}
