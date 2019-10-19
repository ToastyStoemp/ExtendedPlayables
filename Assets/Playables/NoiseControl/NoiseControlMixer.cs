using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace DefaultNamespace
{
    public class NoiseControlMixer : PlayableBehaviour
    {
        protected Transform targetTransform;
        protected Vector3 startPos;

        private bool firstFrameHappened;
        private const float StartSpeed = 0;
        private const float StartIntensity = 0;
        private static readonly Vector3 StartAxis = new Vector3(0,0,0);

        protected virtual void SetupFirstFrame(Transform playableTransform)
        {
            targetTransform = playableTransform;
            startPos = targetTransform.localPosition;

            firstFrameHappened = true;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (playerData is Transform playableTransform)
            {

                if (!firstFrameHappened)
                {
                    SetupFirstFrame(playableTransform);
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

                float resultSpeed = blendedSpeed + StartSpeed * remainingWeight;
                float resultIntensity = blendedIntensity + StartIntensity * remainingWeight;
                Vector3 resultAxis = blendedAxis + StartAxis * remainingWeight;
                
                ApplyNoise(normalisedTime, resultSpeed, resultIntensity, resultAxis);
            }
        }

        protected virtual void ApplyNoise(float time, float speed, float intensity, Vector3 axis)
        {
            Vector3 randomPoint = Random.insideUnitSphere;
            randomPoint.x *= axis.x;
            randomPoint.y *= axis.y;
            randomPoint.z *= axis.z;
                
            targetTransform.localPosition = startPos + (time + speed) * (intensity / 100f) * randomPoint;
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            firstFrameHappened = false;
            if (targetTransform != null)
            {
                targetTransform.localPosition = startPos;
            }
        }
    }
}