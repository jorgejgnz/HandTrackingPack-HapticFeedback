using UnityEngine;
using Zinnia.Tracking.Velocity;

namespace JorgeJGnz
{
    public class KinematicVelocityTracker : VelocityTracker
    {
        [Tooltip("The anchor from which tracking velocity for.")]
        public Rigidbody kinematicRigidbody;

        [Header("Debugging")]
        public Vector3 angularVelocity;
        public Vector3 velocity;

        Quaternion previousRotation;
        Quaternion currentRotation;

        Vector3 previousPosition;
        Vector3 currentPosition;

        //Initialize rotationLast in start, or it will cause an error
        void Start()
        {
            previousRotation = kinematicRigidbody.rotation;
            previousPosition = kinematicRigidbody.position;
        }

        void Update()
        {
            angularVelocity = CalculateAngularVelocity();
            velocity = CalculateVelocity();
        }

        public Vector3 CalculateAngularVelocity()
        {
            currentRotation = kinematicRigidbody.rotation;
            Quaternion deltaRotation = currentRotation * Quaternion.Inverse(previousRotation);
            previousRotation = currentRotation;
            deltaRotation.ToAngleAxis(out var angle, out var axis);
            angle *= Mathf.Deg2Rad;
            return (1.0f / Time.deltaTime) * angle * axis;
        }

        public Vector3 CalculateVelocity()
        {
            currentPosition = kinematicRigidbody.position;
            Vector3 media = (currentPosition - previousPosition);
            Vector3 velocity = media / Time.deltaTime;
            previousPosition = currentPosition;
            return velocity;
        }

        protected override Vector3 DoGetAngularVelocity()
        {
            return angularVelocity;
        }

        protected override Vector3 DoGetVelocity()
        {
            return velocity;
        }

    }
}