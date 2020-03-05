using Malimbe.PropertySerializationAttribute;
using Malimbe.XmlDocumentationAttribute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK.Prefabs.Interactions.Controllables;
using Zinnia.Action;
using Zinnia.Data.Operation.Extraction;

namespace LoUISE
{
    [RequireComponent(typeof(Rigidbody))]
    public class PressureVector3Extractor : Vector3Extractor
    {
        void OnCollisionStay(Collision collision)
        {
            Result = collision.impulse;
        }

        void OnCollisionExit(Collision collision)
        {
            Result = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }
}
