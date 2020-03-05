using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class TriggerActionCallBack : UnityEvent<GameObject>
{
}

[Serializable]
public class TriggerActionCallBackMultiple : UnityEvent<GameObject,GameObject>
{
}


public enum TriggerActionResponse
{
    ReturnMyself,
    ReturnVisitor
}

[RequireComponent(typeof(Collider))]
public class TriggerAction : MonoBehaviour
{
    public bool onlyOneAtOnce = true;
    bool occupied = false;

    [Tooltip("Empty colliderToDetect -> Detect everything")]
    public List<Collider> collidersToDetect = new List<Collider>();

    [Tooltip("If visitor match collider to detect, then return this")]
    public GameObject gameObjectToReturn;

    [Tooltip("If there is no game object defined to be returned on math do this")]
    public TriggerActionResponse ifGameObjectToReturnIsEmpty = TriggerActionResponse.ReturnMyself;

    public TriggerActionCallBack onEnter;
    public TriggerActionCallBack onStay;
    public TriggerActionCallBack onExit;

    public TriggerActionCallBackMultiple onEnterMultiple;
    public TriggerActionCallBackMultiple onStayMultiple;
    public TriggerActionCallBackMultiple onExitMultiple;

    private void OnTriggerEnter(Collider other)
    {
        if (!(occupied && onlyOneAtOnce))
        {
            if (collidersToDetect.Count > 0)
            {
                if (collidersToDetect.Contains(other) && gameObjectToReturn == null)
                {
                    occupied = true;
                    switch (ifGameObjectToReturnIsEmpty)
                    {
                        case TriggerActionResponse.ReturnMyself:
                            onEnter.Invoke(gameObject);
                            break;
                        case TriggerActionResponse.ReturnVisitor:
                            onEnter.Invoke(other.gameObject);
                            break;
                    }
                    onEnterMultiple.Invoke(gameObject,other.gameObject);

                }
                if (collidersToDetect.Contains(other) && gameObjectToReturn != null)
                {
                    occupied = true;
                    onEnter.Invoke(gameObjectToReturn);
                    onEnterMultiple.Invoke(gameObject, other.gameObject);
                }
            }
            else
            {
                if (gameObjectToReturn == null)
                {
                    occupied = true;
                    switch (ifGameObjectToReturnIsEmpty)
                    {
                        case TriggerActionResponse.ReturnMyself:
                            onEnter.Invoke(gameObject);
                            break;
                        case TriggerActionResponse.ReturnVisitor:
                            onEnter.Invoke(other.gameObject);
                            break;
                    }
                    onEnterMultiple.Invoke(gameObject, other.gameObject);

                }
                else
                {
                    occupied = true;
                    onEnter.Invoke(gameObjectToReturn);
                    onEnterMultiple.Invoke(gameObject, other.gameObject);
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (collidersToDetect.Count > 0)
        {
            if (collidersToDetect.Contains(other) && gameObjectToReturn == null)
            {
                switch (ifGameObjectToReturnIsEmpty)
                {
                    case TriggerActionResponse.ReturnMyself:
                        onStay.Invoke(gameObject);
                        break;
                    case TriggerActionResponse.ReturnVisitor:
                        onStay.Invoke(other.gameObject);
                        break;
                }
                onStayMultiple.Invoke(gameObject, other.gameObject);
            }
            if (collidersToDetect.Contains(other) && gameObjectToReturn != null)
            {
                onStay.Invoke(gameObjectToReturn);
                onStayMultiple.Invoke(gameObject, other.gameObject);
            }
        }
        else
        {
            if (gameObjectToReturn == null)
            {
                switch (ifGameObjectToReturnIsEmpty)
                {
                    case TriggerActionResponse.ReturnMyself:
                        onStay.Invoke(gameObject);
                        break;
                    case TriggerActionResponse.ReturnVisitor:
                        onStay.Invoke(other.gameObject);
                        break;
                }
                onStayMultiple.Invoke(gameObject, other.gameObject);
            }
            else
            {
                onStay.Invoke(gameObjectToReturn);
                onStayMultiple.Invoke(gameObject, other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (occupied)
        {
            if (collidersToDetect.Count > 0)
            {
                if (collidersToDetect.Contains(other) && gameObjectToReturn == null)
                {
                    occupied = false;
                    switch (ifGameObjectToReturnIsEmpty)
                    {
                        case TriggerActionResponse.ReturnMyself:
                            onExit.Invoke(gameObject);
                            break;
                        case TriggerActionResponse.ReturnVisitor:
                            onExit.Invoke(other.gameObject);
                            break;
                    }
                    onExitMultiple.Invoke(gameObject, other.gameObject);
                }
                if (collidersToDetect.Contains(other) && gameObjectToReturn != null)
                {
                    occupied = false;
                    onExit.Invoke(gameObjectToReturn);
                    onExitMultiple.Invoke(gameObject, other.gameObject);
                }
            }
            else
            {
                if (gameObjectToReturn == null)
                {
                    occupied = false;
                    switch (ifGameObjectToReturnIsEmpty)
                    {
                        case TriggerActionResponse.ReturnMyself:
                            onExit.Invoke(gameObject);
                            break;
                        case TriggerActionResponse.ReturnVisitor:
                            onExit.Invoke(other.gameObject);
                            break;
                    }
                    onExitMultiple.Invoke(gameObject, other.gameObject);
                }
                else
                {
                    occupied = false;
                    onExit.Invoke(gameObjectToReturn);
                    onExitMultiple.Invoke(gameObject, other.gameObject);
                }
            }
        }
    }


}