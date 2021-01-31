//All credit of this script goes towards Code Monkey

using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryRequestMarker : MonoBehaviour
{
    public Camera UiCamera;
    public DeliverySpot deliverySpot = null;
    public Transform marker;
    public Transform arrow;
    public float borderSize = 100f;

    // Update is called once per frame
    void Update()
    {
        UpdateArrow();
        UpdateMarker();
    }

    private void UpdateArrow()
    {
        if (deliverySpot != null)
        {
            Vector3 toPosition = deliverySpot.transform.position;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0;
            float angle = Mathf.Acos(toPosition.x * fromPosition.x + toPosition.y * fromPosition.y) / (Mathf.Sqrt((Mathf.Pow(toPosition.x, 2) + Mathf.Pow(toPosition.y, 2)) * Mathf.Sqrt(Mathf.Pow(fromPosition.x, 2) + Mathf.Pow(fromPosition.y, 2))));
            arrow.localEulerAngles = new Vector3(0, 0, angle);
        }
    }

    private void UpdateMarker()
    {
        Vector3 sp = Camera.main.WorldToScreenPoint(deliverySpot.transform.position);
        bool isOffScreen = false;
        if (sp.x <= borderSize || sp.x >= Screen.width - borderSize || sp.y <= borderSize || sp.y >= Screen.height - borderSize)
        {
            isOffScreen = true;
        }
        if (isOffScreen)
        {
            marker.gameObject.SetActive(true);
            Vector3 targetSP = sp;
            if (targetSP.x <= borderSize) targetSP.x = borderSize;
            if (targetSP.x >= Screen.width - borderSize) targetSP.x = Screen.width - borderSize;
            if (targetSP.y <= borderSize) targetSP.y = borderSize;
            if (targetSP.y >= Screen.height - borderSize) targetSP.y = Screen.height - borderSize;

            Vector3 markerWorldPosition = UiCamera.WorldToScreenPoint(targetSP);
            marker.position = markerWorldPosition;
        }
        else
        {
            marker.gameObject.SetActive(false);
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
