//Written by Gabriel Tupy 1-30-3031

using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryRequestMarker : MonoBehaviour
{
    private DeliverySpot prevDeliverySpot = null;
    public DeliverySpot deliverySpot = null;
    public Transform arrow;

    // Update is called once per frame
    void Update()
    {
        if (prevDeliverySpot != deliverySpot)
        {
            UnsubscribeToDeliverySpot(prevDeliverySpot);
            SubscribeToDeliverySpot(deliverySpot);
            prevDeliverySpot = deliverySpot;
        }
        UpdateArrow();
    }

    private void UpdateArrow()
    {
        if (deliverySpot != null)
        {
            Vector3 direction = this.transform.position - deliverySpot.transform.position;
            arrow.LookAt(deliverySpot.transform);
        }
    }

    private void EnableMarker(object source)
    {
        this.gameObject.SetActive(true);
    }

    private void DisableMarker(object source)
    {
        this.gameObject.SetActive(false);
    }

    private void UpdateMarker(Vector2 point)
    {
        this.transform.position = point;
    }

    private void SubscribeToDeliverySpot(DeliverySpot reference)
    {
        if (reference != null)
        {
            reference.OnDeliveryRequested += EnableMarker;
            reference.OnDeliveryRequested += DisableMarker;
            reference.OnUpdateDeliveryMarker += UpdateMarker;
        }
    }

    private void UnsubscribeToDeliverySpot(DeliverySpot reference)
    {
        if (reference != null)
        {
            reference.OnDeliveryRequested -= EnableMarker;
            reference.OnDeliveryRequested -= DisableMarker;
            reference.OnUpdateDeliveryMarker -= UpdateMarker;
        }
    }

    private void OnEnable()
    {
        SubscribeToDeliverySpot(deliverySpot);
    }

    private void OnDisable()
    {
        UnsubscribeToDeliverySpot(deliverySpot);
    }
}
