using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static OVRHand;

public class CustomHand : MonoBehaviour
{
    public OVRHand hand;
    OVRSkeleton skeleton;
    OVRMeshRenderer mr;

    public GameObject IndexTipObject;
    public GameObject MiddleTipObject;
    public GameObject RingTipObject;
    public GameObject PinkyTipObject;
    public GameObject ThumbTipObject;
    public GameObject PalmObject;
    OVRBone indexTipBone, middleTipBone, ringTipBone, pinkyTipBone, thumbTipBone, handBone;

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
                IndexTipObject.SetActive(true);
                PalmObject.SetActive(true);
                skeleton.enabled = true;
                mr.enabled = true;
            }
            else{
                // TRACKING IS DISABLED
                IndexTipObject.SetActive(false);
                PalmObject.SetActive(false);
                skeleton.enabled = false;
                mr.enabled = false;
            }
            _tracking = value;
        }
    }
    public bool _tracking;

    public bool smoothIndexTip = true;
    public int smoothSteps = 3;
    List<Vector3> lastPositions = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        skeleton = hand.GetComponent<OVRSkeleton>();
        mr = hand.GetComponent<OVRMeshRenderer>();

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

        if (hand.HandConfidence == TrackingConfidence.High && hand.IsTracked && !tracking) tracking = true;
        else if ((hand.HandConfidence != TrackingConfidence.High || !hand.IsTracked) && tracking) tracking = false;

        if (tracking)
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

            PalmObject.transform.position = handBone.Transform.position;
            PalmObject.transform.rotation = handBone.Transform.rotation;

            isIndexFingerPinching = hand.GetFingerIsPinching(HandFinger.Index);
            ringFingerPinchStrength = hand.GetFingerPinchStrength(HandFinger.Ring);
            thumbFingerPinchStrength = hand.GetFingerPinchStrength(HandFinger.Thumb);

            tmpro.text = "Finger pitching? " + isIndexFingerPinching + "\n";
            tmpro.text += "Ring strength? " + ringFingerPinchStrength + "\n";
            tmpro.text += "Thumb strength? " + thumbFingerPinchStrength + "\n";
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
