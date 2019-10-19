using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DefaultNamespace
{
    public class NoiseMultiControlClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private NoiseMultiControlBehaviour template = new NoiseMultiControlBehaviour();
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<NoiseMultiControlBehaviour>.Create(graph, template);
        }

        public ClipCaps clipCaps => ClipCaps.Blending;
    }
}