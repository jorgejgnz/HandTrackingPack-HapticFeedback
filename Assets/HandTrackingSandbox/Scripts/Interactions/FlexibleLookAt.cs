using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlexibleLookAt : MonoBehaviour
{
    public Transform target;
    public Transform receiver;
    public bool looking = false;

    public Vector3 weights = new Vector3(1f,1f,1f);

    [Header("Locked axes")]
    public bool x;
    public bool y;
    public bool z;

    Vector3 lookingVector;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (looking)
        {
            lookingVector = target.position;

            lookingVector = Vector3.Scale(lookingVector,weights);

            if (x) lookingVector.x = receiver.position.x;
            if (y) lookingVector.y = receiver.position.y;
            if (z) lookingVector.z = receiver.position.z;

            receiver.transform.LookAt(lookingVector);
        }
    }

    public void SetTarget(GameObject go)
    {
        target = go.transform;
    }

    public void SetLooking(bool b)
    {
        looking = b;
    }

}
