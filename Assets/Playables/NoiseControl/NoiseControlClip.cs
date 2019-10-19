using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace DefaultNamespace
{
    public class NoiseControlClip : PlayableAsset, ITimelineClipAsset
    {
        [SerializeField] private NoiseControlBehaviour template = new NoiseControlBehaviour();
        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<NoiseControlBehaviour>.Create(graph, template);
        }

        public ClipCaps clipCaps => ClipCaps.Blending;
    }
}