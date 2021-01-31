using System;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCoordinator : MonoBehaviour
{
    [SerializeField] private GameObject referenceObject = null;
    private ICallParticleEvents reference = null;

    private void CreateParticles(Vector2 location)
    { 
        
    }

    private void SubscribeToICallAnimateEvents(ICallParticleEvents reference)
    {
        if (reference != null)
        {
            reference.CallParticles += CreateParticles;
        }
    }

    private void UnsubscribeToICallAnimateEvents(ICallParticleEvents reference)
    {
        if (reference != null)
        {
            reference.CallParticles -= CreateParticles;
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
