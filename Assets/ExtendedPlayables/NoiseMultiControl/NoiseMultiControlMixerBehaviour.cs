using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Random = UnityEngine.Random;

namespace ExtendedPlayables
{
    public struct SmallTransform
    {
        public Vector3 pos;
        public Quaternion rot;
        public Vector3 scale;
    }

    public class NoiseMultiControlMixerBehaviour : NoiseControlMixerBehaviour
    {
        protected Dictionary<Transform, SmallTransform> transformWithStartPositionMap = new Dictionary<Transform, SmallTransform>();
        public bool alsoApplyOnRoot;
        public bool applyCustomDepth;
        public int customChildDepth;

        protected override void SetupFirstFrame(Transform playableTransform)
        {
            base.SetupFirstFrame(playableTransform);

            transformWithStartPositionMap.Clear();

            if (!applyCustomDepth)
            {
                foreach (Transform child in targetTransform)
                {
                    transformWithStartPositionMap.Add(child, new SmallTransform
                    {
                        pos = child.localPosition,
                        rot = child.localRotation,
                        scale = child.localScale
                    });
                }
            }
            else
            {
                GetChildTransforms(targetTransform, customChildDepth - 1);
            }
        }

        private void GetChildTransforms(Transform currentParent, int depth)
        {
            if (depth == 0)
            {
                foreach (Transform child in currentParent)
                {
                    transformWithStartPositionMap.Add(child, new SmallTransform
                    {
                        pos = child.localPosition,
                        rot = child.localRotation,
                        scale = child.localScale
                    });
                }
            }
            else
            {
                foreach (Transform child in currentParent)
                {
                    GetChildTransforms(child, depth - 1);
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

            foreach (KeyValuePair<Transform, SmallTransform> keyValuePair in transformWithStartPositionMap)
            {
                Vector3 randomPoint = Random.insideUnitSphere;
                randomPoint.x *= axis.x;
                randomPoint.y *= axis.y;
                randomPoint.z *= axis.z;

                if (noiseMode.HasFlag(NoiseMode.Translate))
                {
                    keyValuePair.Key.localPosition = keyValuePair.Value.pos + (time + speed) * (intensity / 100f) * randomPoint;
                }

                if (noiseMode.HasFlag(NoiseMode.Rotate))
                {
                    keyValuePair.Key.localRotation = keyValuePair.Value.rot * Quaternion.Euler(
                                                         Random.Range(rotationRange.x,rotationRange.y) * axis.x * (time + speed) * (intensity / 100f),
                                                         Random.Range(rotationRange.x,rotationRange.y) * axis.y * (time + speed) * (intensity / 100f),
                                                         Random.Range(rotationRange.x,rotationRange.y) * axis.z * (time + speed) * (intensity / 100f));
                }

                if (noiseMode.HasFlag(NoiseMode.Scale))
                {
                    keyValuePair.Key.localScale = keyValuePair.Value.scale + (time + speed) * (intensity / 100f) * randomPoint;
                }
            }
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            base.OnPlayableDestroy(playable);

            if (transformWithStartPositionMap == null)
                return;

            foreach (KeyValuePair<Transform, SmallTransform> keyValuePair in transformWithStartPositionMap)
            {
                if (keyValuePair.Key != null)
                {
                    keyValuePair.Key.localPosition = keyValuePair.Value.pos;
                    keyValuePair.Key.localRotation = keyValuePair.Value.rot;
                    keyValuePair.Key.localScale = keyValuePair.Value.scale;
                }
            }
        }
    }
}