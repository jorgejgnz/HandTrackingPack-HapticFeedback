using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zinnia.Action;
using Zinnia.Rule;

[Serializable]
public class CollisionActionCallBack : UnityEvent<GameObject>
{ }


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CollisionAction : MonoBehaviour
{
    public CollisionActionCallBack onEnter;
    public CollisionActionCallBack onStay;
    public CollisionActionCallBack onExit;

    private void OnCollisionEnter(Collision other)
    {
        onEnter.Invoke(other.gameObject);
    }

    private void OnCollisionStay(Collision other)
    {
        onStay.Invoke(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        onExit.Invoke(other.gameObject);
    }
}
