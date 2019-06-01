using UnityEngine;
using UnityEngine.Playables;
using static TOD_WeatherManager;

namespace MC.Core
{
    [System.Serializable]
    public class TimePlayableData : PlayableAsset
    {
        public float hour = 9;

        public float dayLengthInMinute = 20;

        public CloudType Clouds = default(CloudType);


        // Factory method that generates a playable based on this asset
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            TimePlayable myPlayable = new TimePlayable
            {
                hour = hour,
                Clouds = Clouds,
                dayLengthInMinute = dayLengthInMinute
            };

            return ScriptPlayable<TimePlayable>.Create(graph, myPlayable);
        }
    }

}
