using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace ExtendedPlayables
{
    public class NoiseMultiControlMixerBehaviour : NoiseControlMixerBehaviour
    {
        protected Dictionary<Transform, Vector3> transformWithStartPositionMap = new Dictionary<Transform, Vector3>();
        public bool alsoApplyOnRoot;
        public bool applyCustomDepth;
        public int customChildDepth;
        
        protected override void SetupFirstFrame(Transform playableTransform)
        {
            base.SetupFirstFrame(playableTransform);

            transformWithStartPositionMap = new Dictionary<Transform, Vector3>();

            if (!applyCustomDepth)
            {
                foreach (Transform child in targetTransform)
                {
                    transformWithStartPositionMap.Add(child, child.localPosition);
                }
            }
            else
            {
                foreach (Transform child in targetTransform)
                {
                    if (child.childCount > 1)
                    {
                        Transform targetChild = child.GetChild(customChildDepth - 1);
                        transformWithStartPositionMap.Add(targetChild, targetChild.localPosition);
                    }
                }
            }
        }

        protected override void GetDataFromBehaviour(Playable input, out float speed, out float intensity, out Vector3 axis)
        {
            ScriptPlayable<NoiseMultiControlBehaviour> inputPlayable = (ScriptPlayable<NoiseMultiControlBehaviour>) input;
            NoiseMultiControlBehaviour noiseBehaviour = inputPlayable.GetBehaviour();
            
            speed = noiseBehaviour.speed;
            intensity = noiseBehaviour.intensity;
            axis = noiseBehaviour.axis;
        }
        
        protected override void ApplyNoise(float time, float speed, float intensity, Vector3 axis)
        {
            if (alsoApplyOnRoot)
                base.ApplyNoise(time, speed, intensity, axis);
            
            if (transformWithStartPositionMap == null)
                return;
            
            foreach (KeyValuePair<Transform,Vector3> keyValuePair in transformWithStartPositionMap)
            {
                Vector3 randomPoint = Random.insideUnitSphere;
                randomPoint.x *= axis.x;
                randomPoint.y *= axis.y;
                randomPoint.z *= axis.z;
                
                keyValuePair.Key.localPosition = keyValuePair.Value + (time + speed) * (intensity / 100f) * randomPoint;
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);
            
            if (transformWithStartPositionMap == null)
                return;
            
            foreach (KeyValuePair<Transform, Vector3> keyValuePair in transformWithStartPositionMap)
            {
                if (keyValuePair.Key != null)
                    keyValuePair.Key.localPosition = keyValuePair.Value;
            }
        }
    }
}