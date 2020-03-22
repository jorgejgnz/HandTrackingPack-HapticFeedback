using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public string ifHasThisLayer = "Grabbable";
    public Transform spawnHere;

    public void MoveToMe(GameObject go)
    {
        // Collider grabbed is son of the son of the interactor
        // We want to move the whole interactor (not only one if its colliders)
        if (go.layer == LayerMask.NameToLayer(ifHasThisLayer)) go.transform.parent.parent.position = spawnHere.position;
    }
}
