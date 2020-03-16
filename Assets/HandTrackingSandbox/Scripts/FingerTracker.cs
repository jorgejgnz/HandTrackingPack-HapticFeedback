using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static OVRHand;

public class FingerTracker : MonoBehaviour
{
    [Header("Hand-Tracking")]
    public OVRHand trackedHand;
    OVRSkeleton skeleton;
    OVRMeshRenderer mr;
    SkinnedMeshRenderer smr;
    public Vector3 HandBaseLocalEulerRotOffset_HandTracking;

    [Header("Controllers")]
    public OVRTouchSample.Hand simulatedHand;
    public Transform simulatedHandIndexTip;
    public Transform simulatedHandMiddleTip;
    public Transform simulatedHandRingTip;
    public Transform simulatedHandPinkyTip;
    public Transform simulatedHandThumbTip;
    public Transform simulatedBaseHand;
    public Vector3 HandBaseLocalEulerRotOffset_Controller;

    [Header("Custom finger tips")]
    public GameObject IndexTipObject;
    public GameObject MiddleTipObject;
    public GameObject RingTipObject;
    public GameObject PinkyTipObject;
    public GameObject ThumbTipObject;
    public GameObject BaseHandObject;
    OVRBone indexTipBone, middleTipBone, ringTipBone, pinkyTipBone, thumbTipBone, handBone;

    [Header("Events")]
    public UnityEvent onInputChange;
    public UnityEvent onHandTracking;
    public UnityEvent onControllers;

    [Header("Debugging")]
    public TextMeshPro tmpro;
    public bool usesPanel = true;

    [SerializeField]
    public bool tracking
    {
        get { return _tracking; }
        set {

            if (value != _tracking) onInputChange.Invoke();

            if (value)
            { 

                // TRACKING IS ENABLED
                skeleton.enabled = true;
                mr.enabled = true;
                smr.enabled = true;

                simulatedHand.gameObject.SetActive(false);

                onHandTracking.Invoke();

            }
            else{

                // TRACKING IS DISABLED
                skeleton.enabled = false;
                mr.enabled = false;
                smr.enabled = false;

                simulatedHand.gameObject.SetActive(true);

                onControllers.Invoke();

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
            BaseHandObject.transform.eulerAngles = handBone.Transform.eulerAngles;

            // Apply rotation offset if needed
            Vector3 rotationAdapted = BaseHandObject.transform.localEulerAngles;
            rotationAdapted.x = rotationAdapted.x + HandBaseLocalEulerRotOffset_HandTracking.x;
            rotationAdapted.y = rotationAdapted.y + HandBaseLocalEulerRotOffset_HandTracking.y;
            rotationAdapted.z = rotationAdapted.z + HandBaseLocalEulerRotOffset_HandTracking.z;
            BaseHandObject.transform.localEulerAngles = rotationAdapted;

            if (usesPanel)
            {
                tmpro.text = "(" + gameObject.name + ") Rotation offset: " + HandBaseLocalEulerRotOffset_HandTracking;
            }
        }
        else
        {
            IndexTipObject.transform.position = simulatedHandIndexTip.transform.position;
            MiddleTipObject.transform.position = simulatedHandMiddleTip.transform.position;
            RingTipObject.transform.position = simulatedHandRingTip.transform.position;
            PinkyTipObject.transform.position = simulatedHandPinkyTip.transform.position;
            ThumbTipObject.transform.position = simulatedHandThumbTip.transform.position;

            IndexTipObject.transform.rotation = simulatedHandIndexTip.rotation;
            MiddleTipObject.transform.rotation = simulatedHandMiddleTip.rotation;
            RingTipObject.transform.rotation = simulatedHandRingTip.rotation;
            PinkyTipObject.transform.rotation = simulatedHandPinkyTip.rotation;
            ThumbTipObject.transform.rotation = simulatedHandThumbTip.rotation;

            BaseHandObject.transform.position = simulatedBaseHand.position;
            BaseHandObject.transform.eulerAngles = simulatedBaseHand.eulerAngles;

            // Apply rotation offset if needed
            Vector3 rotationAdapted = BaseHandObject.transform.localEulerAngles;
            rotationAdapted.x = rotationAdapted.x + HandBaseLocalEulerRotOffset_Controller.x;
            rotationAdapted.y = rotationAdapted.y + HandBaseLocalEulerRotOffset_Controller.y;
            rotationAdapted.z = rotationAdapted.z + HandBaseLocalEulerRotOffset_Controller.z;
            BaseHandObject.transform.localEulerAngles = rotationAdapted;

            if (usesPanel)
            {
                tmpro.text = "(" + gameObject.name + ") Rotation offset: " + HandBaseLocalEulerRotOffset_Controller;
            }
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
