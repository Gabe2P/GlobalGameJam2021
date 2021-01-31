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
    public Transform target = null;

    // Update is called once per frame
    void Update()
    {
        UpdateArrow();
        UpdateMarker();
    }

    private void UpdateArrow()
    {
        if (target != null)
        {
            Vector3 toPosition = target.position;
            Vector3 fromPosition = Camera.main.transform.position;
            fromPosition.z = 0;
            float angle = CalculateAngle((toPosition - fromPosition).normalized);
            Debug.Log(angle);
            arrow.localEulerAngles = new Vector3(0, 0, angle);
        }
    }

    private float CalculateAngle(Vector3 dir)
    {
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        return angle;
    }

    private void UpdateMarker()
    {
        if (target != null)
        {
            Vector3 sp = Camera.main.WorldToScreenPoint(target.position);
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
                marker.localPosition = new Vector3(marker.localPosition.x, marker.localPosition.y, 0);
            }
            else
            {
                marker.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        if (deliverySpot != null)
        {
            target = deliverySpot.transform;
        }
    }
}
