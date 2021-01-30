//Written By Gabriel Tupy 1-30-2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorCoordinator : MonoBehaviour
{
    [SerializeField] private Animator anim = null;
    [SerializeField] private GameObject referenceObject = null;
    private ICallAnimateEvents reference = null;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        if (referenceObject != null)
        {
            reference = referenceObject.GetComponent<ICallAnimateEvents>();
            SubscribeToICallAnimateEvents(reference);
        }
    }

    private void CallAnimationTrigger(string command, object value)
    {
        if (command == "")
        {
            return;
        }
        foreach (AnimatorControllerParameter parameter in anim.parameters)
        {
            if (parameter.name.Equals(command))
            {
                if (value == null)
                {
                    anim.SetTrigger(command);
                    return;
                }
                if (value.GetType() == typeof(bool))
                {
                    anim.SetBool(command, (bool)value);
                    return;
                }
                if (value.GetType() == typeof(float))
                {
                    anim.SetFloat(command, (float)value);
                    return;
                }
                if (value.GetType() == typeof(int))
                {
                    anim.SetInteger(command, (int)value);
                    return;
                }
            }
        }
    }

    private void SubscribeToICallAnimateEvents(ICallAnimateEvents reference)
    {
        if (reference != null)
        {
            reference.CallAnimationTrigger += CallAnimationTrigger;
        }
    }

    private void UnsubscribeToICallAnimateEvents(ICallAnimateEvents reference)
    {
        if (reference != null)
        {
            reference.CallAnimationTrigger -= CallAnimationTrigger;
        }
    }

    private void OnEnable()
    {
        SubscribeToICallAnimateEvents(reference);
    }

    private void OnDisable()
    {
        UnsubscribeToICallAnimateEvents(reference);
    }
}
