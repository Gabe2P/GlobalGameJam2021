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
}
