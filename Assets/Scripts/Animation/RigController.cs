
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace CGD
{
    public class RigController : MonoBehaviour
    {
        [SerializeField] private Rig animRig;

        [Header("Rig Targets")]
        [SerializeField] private ParentConstraint aimTarget;
        [SerializeField] private ParentConstraint lhTarget;
        [SerializeField] private ParentConstraint lhRotation;

        [Header("Skeleton Slots")]
        [SerializeField] private Transform toolContainer; 

        public Transform ToolParent { get { return toolContainer; } }

        public void UpdateRigWeight(float weight) => animRig.weight = weight;

        public void SetConstraintTargets(Transform leftHand, Transform camera)
        {
            aimTarget.Source = camera;
            lhTarget.Source = leftHand;
            lhRotation.Source = leftHand;
        }



    }
}
