using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD
{
    public class ParentConstraint : MonoBehaviour
    {
        [SerializeField] private Transform source;
        [SerializeField] private float smoothTime;

        [Header("Constraint Settings")]
        [SerializeField] private Vector3 positionOffset;
        [SerializeField] private Vector3 rotationOffset;

        private Vector3 velocity;
        private Vector3 targetPosition;
        private Quaternion relativeRotationOffset;

        public Transform Source {  get { return source; } set { source = value; } }

        private void Start()
        {
            relativeRotationOffset = Quaternion.Euler(rotationOffset);
        }

        private void Update()
        {
            if (source != null) 
            {
                targetPosition = source.position;

                if(positionOffset != Vector3.zero) 
                {
                    targetPosition += source.right * positionOffset.x;
                    targetPosition += source.up * positionOffset.y;
                    targetPosition += source.forward * positionOffset.z;
                    
                }

                transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity,
                    smoothTime * Time.deltaTime);

                transform.rotation = source.rotation * relativeRotationOffset;

                //float delta = Quaternion.Angle(transform.rotation, target.rotation);
                //if (delta > 0f)
                //{
                //    float t = Mathf.SmoothDampAngle(delta, 0.0f, ref rotationVelocity, smoothTime);
                //    t = 1.0f - (t / delta);
                //    transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, t);
                //}
            }
        }


    }
}
