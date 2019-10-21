using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ExtendedPlayables
{
    [TrackColor(1,0,0)]
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(NoiseControlClip))]
    public class NoiseControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<NoiseControlMixerBehaviour>.Create(graph, inputCount);
        }
    }
}