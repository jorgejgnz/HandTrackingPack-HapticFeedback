using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public struct Condition
{
    public GameObject ifThis;
    public UnityEvent thenDo;
}

public class PanelManager : MonoBehaviour
{
    int currentItem;

    [Header("Buttons")]
    public GameObject matchingButton;
    public GameObject previousButton;
    public GameObject nextButton;

    [Header("Interactions")]
    public List<GameObject> interactions;

    [Header("Conditions")]
    [SerializeField]
    public List<Condition> conditions;

    public UnityEvent onChange;

    // Start is called before the first frame update
    void Start()
    {
        EnableInteraction(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PreviousItem()
    {
        currentItem--;

        if (currentItem < 0) currentItem = interactions.Count - 1;

        EnableInteraction(currentItem);
    }

    public void NextItem()
    {
        currentItem++;

        if (currentItem > interactions.Count - 1) currentItem = 0;

        EnableInteraction(currentItem);
    }

    public void EnableInteraction(int item)
    {
        onChange.Invoke();

        if (item >= 0 && item < interactions.Count)
        {
            DisableAllInteractions();
            interactions[item].SetActive(true);
            currentItem = item;

            for (int i = 0; i < conditions.Count; i++)
            {
                if (interactions[item] == conditions[i].ifThis)
                {
                    conditions[i].thenDo.Invoke();
                }
            }
        }
        else throw new System.Exception("You're trying to enable a non-existing interaction");
    }

    void DisableAllInteractions()
    {
        foreach (GameObject go in interactions)
        {
            go.SetActive(false);
        }
    }
}
