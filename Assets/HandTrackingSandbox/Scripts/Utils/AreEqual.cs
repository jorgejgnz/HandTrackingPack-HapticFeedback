using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zinnia.Action;


namespace LoUISE
{
    public class AreEqual : BooleanAction
    {
        public GameObject A, B;

        void Update()
        {
            Receive(A == B);
        }

        /*
         * Needed because:
         *      We will use two components of the same type (AreEqual) for each interactable object (1 per Interactor)
         *      UnityEvents doesn't support multiple components of the same type (it will only Reset the first component AreEqual found)
         */
        public void SetToAll(GameObject go)
        {
            foreach (AreEqual component in this.gameObject.GetComponents<AreEqual>())
            {
                component.A = go;
            }
        }

        /*
         * Needed because:
         *      SetGameobjectToCompare(empty) is not understood
         *      We will use two components of the same type (AreEqual) for each interactable object (1 per Interactor)
         *      UnityEvents doesn't support multiple components of the same type (it will only Reset the first component AreEqual found)
         */
        public void ResetAll()
        {
            foreach (AreEqual component in this.gameObject.GetComponents<AreEqual>())
            {
                component.A = null;
            }
        }
    }
}
