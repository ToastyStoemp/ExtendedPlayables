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
        public bool alsoApplyOnRoot;
        public bool applyCustomDepth;
        public int customChildDepth = 2;
        
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<NoiseMultiControlMixerBehaviour> noiseMixer = ScriptPlayable<NoiseMultiControlMixerBehaviour>.Create(graph, inputCount);
            NoiseMultiControlMixerBehaviour noiseMixerBehaviour = noiseMixer.GetBehaviour();
            noiseMixerBehaviour.alsoApplyOnRoot = alsoApplyOnRoot;
            noiseMixerBehaviour.applyCustomDepth = applyCustomDepth;
            noiseMixerBehaviour.customChildDepth = customChildDepth;
            return noiseMixer;
        }
    }
}