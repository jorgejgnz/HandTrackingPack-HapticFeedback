using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollableSurface : MonoBehaviour
{

    public Collider trigger;

    public MeshRenderer mr;

    Transform fingerToFollow = null;

    public float offsetX = 0f, offsetY = 0f;
    public float weightX = -1.5f, weightY = -3f;

    Vector2 lastPos, currentPos, offset;

    Vector2 initialTiling;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector2(0f,0f);
        initialTiling = mr.material.mainTextureScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (fingerToFollow != null)
        {
            currentPos = new Vector2(transform.InverseTransformPoint(fingerToFollow.position).x, transform.InverseTransformPoint(fingerToFollow.position).y);

            offset += currentPos - lastPos;

            mr.material.mainTextureOffset = new Vector2(weightX * offset.x + offsetX, weightY * offset.y + offsetY);

            lastPos = currentPos;
        }
    }

    public void onEnter(GameObject go)
    {
        if (fingerToFollow == null)
        {
            fingerToFollow = go.transform;
            lastPos = new Vector2(fingerToFollow.position.x,fingerToFollow.position.z);
        }
    }

    public void onStay(GameObject go)
    {
        if (fingerToFollow != null && go.transform == fingerToFollow)
        {

        }
    }

    public void onExit(GameObject go)
    {
        if (fingerToFollow != null && go.transform == fingerToFollow)
        {
            fingerToFollow = null;
        }
    }
}
