﻿using UnityEngine;
using UnityEngine.Playables;

namespace MC.Core
{
    // A behaviour that is attached to a playable
    public class SeasonPlayable : PlayableBehaviour
    {
        public CinematicSeason.Season season = CinematicSeason.Season.Autumn;

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
            var cinematic = GameObject.FindObjectOfType<CinematicSeason>();
            cinematic?.OnSeasonChanged(season);
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