using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zinnia.Action;

namespace JorgeJGnz
{
    public class SequentialEvent : BooleanAction
    {
        // This BooleanAction will be True while its Sequence is in process

        [Header("Phase 0")]
        public BooleanAction primaryCondition;
        public float checkingDelay;
        public UnityEngine.Events.UnityEvent onSequenceStarted;
        bool started = false;

        [Header("Phase 1")]
        public BooleanAction secondaryCondition;
        public float validationDelay;

        [Header("Phase 2")]
        public UnityEngine.Events.UnityEvent onSuccess;
        public UnityEngine.Events.UnityEvent onFailure;

        // Update is called once per frame
        void Update()
        {
            if (primaryCondition && !started)
            {
                started = true;
                onSequenceStarted.Invoke();
                StartCoroutine(SecondaryActionChecking(checkingDelay));
            }

            Receive(started);
        }

        IEnumerator SecondaryActionChecking(float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);
            if (secondaryCondition)
            {
                StartCoroutine(SecondaryActionValidation(validationDelay));
            }
            else
            {
                onFailure.Invoke();
                started = false;
            }
        }

        IEnumerator SecondaryActionValidation(float delayInSeconds)
        {
            yield return new WaitForSeconds(delayInSeconds);

            if (secondaryCondition) onSuccess.Invoke();
            else onFailure.Invoke();

            started = false;
        }

        public void Interrupt()
        {
            StopAllCoroutines();
            started = false;
        }
    }
}
