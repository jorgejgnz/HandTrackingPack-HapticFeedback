using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;

public class ItemExtractionAction : BooleanAction
{
    public BooleanAction inExtractionArea;
    public BooleanAction isGripping;
    public BooleanAction isGrabbing;

    void Update()
    {
        Receive(inExtractionArea.Value && isGripping.Value && !isGrabbing.Value);
    }
}
