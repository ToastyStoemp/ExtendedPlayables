using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ExtendedPlayables
{
    [TrackColor(1,0,0)]
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(NoiseMultiControlClip))]
    public class NoiseMultiControlTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<NoiseMultiControlMixerBehaviourBehaviour>.Create(graph, inputCount);
        }
    }
}