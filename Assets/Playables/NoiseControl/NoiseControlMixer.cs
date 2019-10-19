using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class NoiseControlMixer : PlayableBehaviour
    {
        private Transform targetTransform;
        private Vector3 startPos;

        private bool firstFrameHappened;
        private float startSpeed = 0;
        private float startIntensity = 0;
        private Vector3 startAxis = Vector3.zero;
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (playerData is Transform playableTransform)
            {

                if (!firstFrameHappened)
                {
                    targetTransform = playableTransform;
                    startPos = targetTransform.localPosition;

                    firstFrameHappened = true;
                }
                
                int inputCount = playable.GetInputCount();

                float blendedSpeed = 0f;
                float blendedIntensity = 0f;
                Vector3 blendedAxis = Vector3.zero;

                float totalWeight = 0f;

                for (int i = 0; i < inputCount; i++)
                {
                    float inputWeight = playable.GetInputWeight(i);
                    ScriptPlayable<NoiseControlBehaviour> inputPlayable = (ScriptPlayable<NoiseControlBehaviour>) playable.GetInput(i);
                    NoiseControlBehaviour noiseBehaviour = inputPlayable.GetBehaviour();

                    blendedSpeed += noiseBehaviour.speed * inputWeight;
                    blendedIntensity += noiseBehaviour.intensity * inputWeight;
                    blendedAxis += noiseBehaviour.axis * inputWeight;

                    totalWeight += inputWeight;
                }

                float normalisedTime = (float) playable.GetTime();
                float remainingWeight = 1 - totalWeight;

                float blendedResultSpeed = blendedSpeed + startSpeed * remainingWeight;
                float blendedResultIntensity = blendedIntensity + startIntensity * remainingWeight;
                Vector3 blendedResultAxis = blendedAxis + startAxis * remainingWeight;
                
                Vector3 randomPoint = Random.insideUnitSphere;
                randomPoint.x *= blendedResultAxis.x;
                randomPoint.y *= blendedResultAxis.y;
                randomPoint.z *= blendedResultAxis.z;
                
                targetTransform.localPosition = startPos + (normalisedTime + blendedResultSpeed) * (blendedResultIntensity / 100f) * randomPoint;
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            firstFrameHappened = false;
            targetTransform.localPosition = startPos;
        }
    }
}