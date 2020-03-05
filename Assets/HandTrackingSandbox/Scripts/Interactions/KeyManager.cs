using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FingerKeyInteraction
{
    //Variable declaration
    //Note: I'm explicitly declaring them as public, but they are public by default. You can use private if you choose.
    public GameObject key;
    public GameObject finger;

    public FingerKeyInteraction(GameObject key, GameObject finger)
    {
        this.finger = finger;
        this.key = key;
    }
}

public class KeyManager : MonoBehaviour
{
    public GameObject keysContainer;

    public List<FingerKeyInteraction> interactions = new List<FingerKeyInteraction>();

    public int interactionsCount = 0;

    public void FingerEnter(GameObject key, GameObject finger)
    {
        for (int i = 0; i < interactions.Count; i++)
        {
            // One key can only be pressed by one finger at once
            // One finger can only press one key at once
            if (interactions[i].key == key || interactions[i].finger == finger) return;

        }

        interactions.Add(new FingerKeyInteraction(key,finger));
        key.GetComponentInChildren<AudioSource>().Play();
        UpdateArrays();
        return;
    }

    public void FingerExit(GameObject key, GameObject finger)
    {
        for (int i = 0; i < interactions.Count; i++)
        {
            if (interactions[i].key == key && interactions[i].finger == finger)
            {
                interactions.RemoveAt(i);
                key.GetComponentInChildren<AudioSource>().Stop();

                UpdateArrays();
                return;
            }
        }
    }

    void UpdateArrays()
    {
        interactionsCount = interactions.Count;
    }

}
