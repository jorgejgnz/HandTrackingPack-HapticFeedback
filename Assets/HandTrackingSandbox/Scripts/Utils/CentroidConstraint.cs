using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum State
{
    Undefined,
    MinReached,
    Nominal,
    MaxReached
}

public class CentroidConstraint : MonoBehaviour
{
    [Header("Place this")]
    public Transform receiver;

    [Header("Between this")]
    public Transform A;
    public Transform B;

    [Header("Rotation")]
    public Transform lookAt;

    [Header("Theresolds")]
    public float minTheresold = 0.2f;
    public float maxTheresold = 0.5f;

    [Header("Events")]
    public UnityEvent onMinReached;
    public UnityEvent onNominalReached;
    public UnityEvent onMaxReached;
    State state = State.Undefined;

    [Header("Debugging")]
    public float currentNormalizedValue;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        receiver.transform.position = (A.position + B.position) / 2.0f;
        if (lookAt != null) receiver.LookAt(lookAt);

        if (Vector3.Distance(A.position, B.position) < minTheresold)
        {
            currentNormalizedValue = 0.0f;

            if (state != State.MinReached)
            {
                state = State.MinReached;
                onMinReached.Invoke();
            }
        }
        else if (Vector3.Distance(A.position, B.position) > minTheresold && Vector3.Distance(A.position, B.position) < maxTheresold)
        {
            currentNormalizedValue = (Vector3.Distance(A.position, B.position) - minTheresold) / (maxTheresold - minTheresold);

            if (state != State.Nominal)
            {
                state = State.Nominal;
                onNominalReached.Invoke();
            }
        }
        else if (Vector3.Distance(A.position, B.position) > maxTheresold)
        {
            currentNormalizedValue = 1.0f;

            if (state != State.MaxReached)
            {
                state = State.MaxReached;
                onMaxReached.Invoke();
            }
        }
    }

    Vector3 findCentroid(List<Transform> list)
    {
        Vector3 center = Vector3.zero;
        float count = 0;
        for (int i = 0; i < list.Count; i++)
        {
            center += list[i].transform.position;
            count++;
        }

        return center / count;
    }
}
