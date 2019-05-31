using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using static TOD_WeatherManager;

namespace MC.Core
{
    // A behaviour that is attached to a playable
    public class TimePlayable : PlayableBehaviour
    {
        public float hour;

        public CloudType Clouds = default(CloudType);

        // Called when the owning graph starts playing
        public override void OnGraphStart(Playable playable)
        {

        }

        // Called when the owning graph stops playing
        public override void OnGraphStop(Playable playable)
        {

        }

        // Called when the state of the playable is set to Play
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            var sky = GameObject.FindObjectOfType<TOD_Sky>();
            sky.Cycle.Hour = hour;

            var weather = GameObject.FindObjectOfType<TOD_WeatherManager>();
            weather.Clouds = Clouds;

            sky.UpdateAmbient();
        }

        // Called when the state of the playable is set to Paused
        public override void OnBehaviourPause(Playable playable, FrameData info)
        {

        }

        // Called each frame while the state is set to Play
        public override void PrepareFrame(Playable playable, FrameData info)
        {

        }
    }

}
