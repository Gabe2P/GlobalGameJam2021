//Written By Gabriel Tupy 1-30-2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour, ICallVFXEvents
{
    public event Action<Vector2, Quaternion, float> CallVFX;

    [SerializeField] private VFXCoordinator vfxCoordinator = null;
    [SerializeField] private float VFXLifeTime = .1f;
    [SerializeField] private float movementRequired = .3f;

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.transform.gameObject.GetComponentInParent<Rigidbody2D>();
        if (rb != null)
        {
            if (rb.velocity.magnitude > movementRequired)
            {
                Debug.Log("Im Being Called");
                CallVFX?.Invoke(other.transform.position, Quaternion.identity, VFXLifeTime);
            }
            if (rb.tag.Equals("Player"))
            {
                CallVFX?.Invoke(other.transform.position, Quaternion.identity, VFXLifeTime);
            }
        }
    }
}
