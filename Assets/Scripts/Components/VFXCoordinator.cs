using System;
using System.Collections.Generic;
using UnityEngine;

public class VFXCoordinator : MonoBehaviour
{
    [SerializeField] private List<GameObject> referenceObjects = new List<GameObject>();
    [SerializeField] private GameObject vfxPrefab;

    private void CreateVFX(Vector2 location, Quaternion rotation, float lifeTime)
    {
        GameObject clone = Instantiate(vfxPrefab, location, rotation);
        if (lifeTime > 0)
        {
            Destroy(clone, lifeTime);
        }
    }

    private void SubscribeToICallAnimateEvents(ICallVFXEvents reference)
    {
        if (reference != null)
        {
            reference.CallVFX += CreateVFX;
        }
    }

    private void UnsubscribeToICallAnimateEvents(ICallVFXEvents reference)
    {
        if (reference != null)
        {
            reference.CallVFX -= CreateVFX;
        }
    }

    private void OnEnable()
    {
        foreach (GameObject reference in referenceObjects)
        {
            if (reference != null)
            {
                SubscribeToICallAnimateEvents(reference.GetComponent<ICallVFXEvents>());
            }
        }
    }

    private void OnDisable()
    {
        foreach (GameObject reference in referenceObjects)
        {
            if (reference != null)
            {
                UnsubscribeToICallAnimateEvents(reference.GetComponent<ICallVFXEvents>());
            }
        }
    }
}
