using System;
using UnityEngine;
using UnityEngine.Playables;

namespace DefaultNamespace
{
    [Serializable]
    public class NoiseControlBehaviour : PlayableBehaviour
    {
        public float speed = 10;
        public float intensity = 1f;
        public Vector3 axis = Vector3.one;

        private Transform targetTransform;
        private Vector3 startPos;
        
        private bool firstFrameHappened;
        private float startSpeed;
        private float startIntensity;
        private Vector3 startAxis;
    }
}