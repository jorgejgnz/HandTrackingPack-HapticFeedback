using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static OVRHand;

public class FingerTracker : MonoBehaviour
{
    [Header("Hand-Tracking")]
    public OVRHand trackedHand;
    OVRSkeleton skeleton;
    OVRMeshRenderer mr;
    SkinnedMeshRenderer smr;

    [Header("Controllers")]
    public OVRTouchSample.Hand simulatedHand;
    public Transform simulatedHandIndexTip;
    public Transform simulatedHandMiddleTip;
    public Transform simulatedHandRingTip;
    public Transform simulatedHandPinkyTip;
    public Transform simulatedHandThumbTip;
    public Transform simulatedBaseHand;

    [Header("Custom finger tips")]
    public GameObject IndexTipObject;
    public GameObject MiddleTipObject;
    public GameObject RingTipObject;
    public GameObject PinkyTipObject;
    public GameObject ThumbTipObject;
    public GameObject BaseHandObject;
    OVRBone indexTipBone, middleTipBone, ringTipBone, pinkyTipBone, thumbTipBone, handBone;

    [Header("Debugging")]
    public TextMeshPro tmpro;
    bool isIndexFingerPinching;
    float ringFingerPinchStrength;
    float  thumbFingerPinchStrength;

    [SerializeField]
    public bool tracking
    {
        get { return _tracking; }
        set {
        
            if (value)
            {
                // TRACKING IS ENABLED
                skeleton.enabled = true;
                mr.enabled = true;
                smr.enabled = true;

                simulatedHand.gameObject.SetActive(false);

            }
            else{
                // TRACKING IS DISABLED
                skeleton.enabled = false;
                mr.enabled = false;
                smr.enabled = false;

                simulatedHand.gameObject.SetActive(true);

            }
            _tracking = value;
        }
    }
    public bool _tracking;

    [Header("Smoothing")]
    public bool smoothIndexTip = true;
    public int smoothSteps = 3;
    List<Vector3> lastPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        skeleton = trackedHand.GetComponent<OVRSkeleton>();
        mr = trackedHand.GetComponent<OVRMeshRenderer>();
        smr = trackedHand.GetComponent<SkinnedMeshRenderer>();

        foreach (OVRBone bone in skeleton.Bones)
        {
            if (bone.Id == OVRSkeleton.BoneId.Hand_IndexTip)
            {
                indexTipBone = bone;
            }
            if (bone.Id == OVRSkeleton.BoneId.Hand_MiddleTip)
            {
                middleTipBone = bone;
            }
            if (bone.Id == OVRSkeleton.BoneId.Hand_RingTip)
            {
                ringTipBone = bone;
            }
            if (bone.Id == OVRSkeleton.BoneId.Hand_PinkyTip)
            {
               pinkyTipBone = bone;
            }
            if (bone.Id == OVRSkeleton.BoneId.Hand_ThumbTip)
            {
                thumbTipBone = bone;
            }
            if (bone.Id == OVRSkeleton.BoneId.Hand_Start)
            {
                handBone = bone;
            }
        }

        for (int i = 0; i < smoothSteps; i++)
        {
            lastPositions.Add(IndexTipObject.transform.position);
        }


    }

    // Update is called once per frame
    void Update()
    {

        if (trackedHand.IsTracked && !tracking) tracking = true;
        else if (!trackedHand.IsTracked && tracking) tracking = false;

        if (trackedHand.HandConfidence == TrackingConfidence.High && tracking)
        {
            IndexTipObject.transform.position = indexTipBone.Transform.position;
            MiddleTipObject.transform.position = middleTipBone.Transform.position;
            RingTipObject.transform.position = ringTipBone.Transform.position;
            PinkyTipObject.transform.position = pinkyTipBone.Transform.position;
            ThumbTipObject.transform.position = thumbTipBone.Transform.position;

            IndexTipObject.transform.rotation = indexTipBone.Transform.rotation;
            MiddleTipObject.transform.rotation = middleTipBone.Transform.rotation;
            RingTipObject.transform.rotation = ringTipBone.Transform.rotation;
            PinkyTipObject.transform.rotation = pinkyTipBone.Transform.rotation;
            ThumbTipObject.transform.rotation = thumbTipBone.Transform.rotation;

            BaseHandObject.transform.position = handBone.Transform.position;
            BaseHandObject.transform.rotation = handBone.Transform.rotation;

            isIndexFingerPinching = trackedHand.GetFingerIsPinching(HandFinger.Index);
            ringFingerPinchStrength = trackedHand.GetFingerPinchStrength(HandFinger.Ring);
            thumbFingerPinchStrength = trackedHand.GetFingerPinchStrength(HandFinger.Thumb);

            tmpro.text = "Finger pitching? " + isIndexFingerPinching + "\n";
            tmpro.text += "Ring strength? " + ringFingerPinchStrength + "\n";
            tmpro.text += "Thumb strength? " + thumbFingerPinchStrength + "\n";
        }else
        {
            IndexTipObject.transform.position = simulatedHandIndexTip.position;
            MiddleTipObject.transform.position = simulatedHandMiddleTip.position;
            RingTipObject.transform.position = simulatedHandRingTip.position;
            PinkyTipObject.transform.position = simulatedHandPinkyTip.position;
            ThumbTipObject.transform.position = simulatedHandThumbTip.position;

            IndexTipObject.transform.rotation = simulatedHandIndexTip.rotation;
            MiddleTipObject.transform.rotation = simulatedHandMiddleTip.rotation;
            RingTipObject.transform.rotation = simulatedHandRingTip.rotation;
            PinkyTipObject.transform.rotation = simulatedHandPinkyTip.rotation;
            ThumbTipObject.transform.rotation = simulatedHandThumbTip.rotation;

            BaseHandObject.transform.position = simulatedBaseHand.position;
            BaseHandObject.transform.rotation = simulatedBaseHand.rotation;

            tmpro.text = "Hand is not being tracked!";
        }

        if (smoothIndexTip)
        {
            lastPositions.RemoveAt(0);
            lastPositions.Add(IndexTipObject.transform.position);
            IndexTipObject.transform.position = FindCenterPoint(lastPositions);
        }
        
    }

    public Vector3 FindCenterPoint(List<Vector3> gos) {
        Vector3 center = new Vector3(0, 0, 0);
        for (int i = 0; i < gos.Count; i++)
        {
            center += gos[i];
        }
        center = center / gos.Count;
        return center;
    }
}
