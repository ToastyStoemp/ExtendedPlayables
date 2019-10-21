using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace ExtendedPlayables
{
    [Flags]
    public enum NoiseMode
    {
        Translate = 1,
        Rotate = 2,
        Scale = 4
    }
    
    [TrackColor(1,0,0)]
    [TrackBindingType(typeof(Transform))]
    [TrackClipType(typeof(NoiseControlClip))]
    public class NoiseControlTrack : TrackAsset
    {
        public NoiseMode noiseMode = NoiseMode.Translate;
        
        public Vector2 rotationRange = new Vector2(-15, 15);

        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<NoiseControlMixerBehaviour> noiseMixer = ScriptPlayable<NoiseControlMixerBehaviour>.Create(graph, inputCount);
            NoiseControlMixerBehaviour noiseMixerBehaviour = noiseMixer.GetBehaviour();
            noiseMixerBehaviour.noiseMode = noiseMode;
            noiseMixerBehaviour.rotationRange = rotationRange;
            return noiseMixer;
        }
    }
}