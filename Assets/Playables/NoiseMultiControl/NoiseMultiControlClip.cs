using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Playables
{
    public class NoiseMultiControlClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private NoiseMultiControlBehaviour settings = new NoiseMultiControlBehaviour();
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<NoiseMultiControlBehaviour>.Create(graph, settings);
        }

        public ClipCaps clipCaps => ClipCaps.Blending;
    }
}