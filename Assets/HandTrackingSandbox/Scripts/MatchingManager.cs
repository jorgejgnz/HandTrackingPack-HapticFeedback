using OculusSampleFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static OVRHand;
using static OVRBoneCapsule;
using TMPro;
using UnityEngine.Events;

namespace JorgeJGnz
{
    public class MatchingManager : MonoBehaviour
    {
        [Header("OVRHands")]
        public OVRHand left, right;

        OVRSkeleton leftSkeleton, rightSkeleton;

        OVRBone leftIndexTip, rightIndexTip;

        [Header("Objects")]
        public GameObject table;

        public TextMeshPro panel;

        bool canDetect = true;

        [Header("Activation")]
        public Collider toggleButton;

        public UnityEvent onActivation, onDeactivation;

        bool PointingGesture_R = false;
        bool PointingGesture_L = false;
        bool PitchingGesture_R = false;
        bool PitchingGesture_L = false;

        Vector3 rightIndexTipPos, leftIndexTipPos, tablePos;
        Quaternion tableRot;

        private void Start()
        {
            leftSkeleton = left.GetComponent<OVRSkeleton>();
            foreach (OVRBone bone in leftSkeleton.Bones)
            {
                if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                {
                    leftIndexTip = bone;
                }
            }


            rightSkeleton = right.GetComponent<OVRSkeleton>();
            foreach (OVRBone bone in rightSkeleton.Bones)
            {
                if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
                {
                    rightIndexTip = bone;
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            PitchingGesture_L = left.GetFingerIsPinching(HandFinger.Index);
            PitchingGesture_R = right.GetFingerIsPinching(HandFinger.Index);

            if (((PitchingGesture_L && PitchingGesture_R) || (PointingGesture_L && PointingGesture_R))
                && canDetect)
            {
                leftIndexTipPos = leftIndexTip.Transform.position;
                rightIndexTipPos = rightIndexTip.Transform.position;

                // A is set to 0, B is set to -A
                // (B-A)/2 will be median point assuming A is center
                // But A is not center so (B-A)/2 has to me moved to A's position so: MedianPoint = A + (A-B)/2

                tablePos.x = leftIndexTipPos.x + (rightIndexTipPos.x - leftIndexTipPos.x) / 2;
                tablePos.y = leftIndexTipPos.y + (rightIndexTipPos.y - leftIndexTipPos.y) / 2;
                tablePos.z = leftIndexTipPos.z + (rightIndexTipPos.z - leftIndexTipPos.z) / 2;

                Vector3 v = rightIndexTipPos - leftIndexTipPos;
                tableRot = Quaternion.FromToRotation(Vector3.right, v);

                table.transform.position = tablePos;
                table.transform.rotation = tableRot;

                // Forced to stay in plane XZ
                Vector3 r = table.transform.eulerAngles;
                r.x = 0f;
                r.z = 0f;

                table.transform.eulerAngles = r;

            }
        }

        public void ToggleDetect()
        {
            canDetect = !canDetect;
            if (canDetect) onActivation.Invoke();
            else onDeactivation.Invoke();
        }

        public void SetPointingGestureL(bool b)
        {
            PointingGesture_L = b;
        }

        public void SetPointingGestureR(bool b)
        {
            PointingGesture_R = b;
        }
    }
}
