using UnityEngine;
using UnityEngine.Playables;

namespace MC.Core
{
    [System.Serializable]
    public class SeasonPlayableData : PlayableAsset
    {
        public CinematicSeason.Season season = CinematicSeason.Season.Autumn;

        // Factory method that generates a playable based on this asset
        public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
        {
            SeasonPlayable myPlayable = new SeasonPlayable
            {
                season = season
            };

            return ScriptPlayable<SeasonPlayable>.Create(graph, myPlayable);
        }
    }

}
