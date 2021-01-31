//Written Gabriel Tupy By 1-29-2021

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public event Action OnGrabButtonPress;
    public event Action OnGrabButtonRelease;
    public event Action OnGrabButtonHeld;

    public event Action OnDashButtonPress;
    public event Action OnDashButtonRelease;
    public event Action OnDashButtonHeld;

    public event Action OnPauseButtonPress;
    public event Action OnPauseButtonRelease;
    public event Action OnPauseButtonHeld;

    public event Action<Vector2> OnMovementChanged;

    private Vector2 prevVector = Vector2.zero;
    private Vector2 curVector = Vector2.zero;

    [SerializeField] private string XAxisString = "";
    [SerializeField] private string YAxisString = "";
    [SerializeField] private string grabString = "";
    [SerializeField] private string dashString = "";
    [SerializeField] private string pauseString = "";

    private void Update()
    {
        if (Input.GetButtonDown(grabString))
        {
            OnGrabButtonPress?.Invoke();
        }
        if (Input.GetButtonUp(grabString))
        {
            OnGrabButtonRelease?.Invoke();
        }
        if (Input.GetButton(grabString))
        {
            OnGrabButtonHeld?.Invoke();
        }

        if (Input.GetButtonDown(dashString))
        {
            OnDashButtonPress?.Invoke();
        }
        if (Input.GetButtonUp(dashString))
        {
            OnDashButtonRelease?.Invoke();
        }
        if (Input.GetButton(dashString))
        {
            OnDashButtonHeld?.Invoke();
        }

        if (Input.GetButtonDown(pauseString))
        {
            OnPauseButtonPress?.Invoke();
        }
        if (Input.GetButtonUp(pauseString))
        {
            OnPauseButtonRelease?.Invoke();
        }
        if (Input.GetButton(pauseString))
        {
            OnPauseButtonHeld?.Invoke();
        }

        curVector = new Vector2(Input.GetAxisRaw(XAxisString), Input.GetAxisRaw(YAxisString));
        if (curVector != prevVector)
        {
            OnMovementChanged?.Invoke(curVector);
            prevVector = curVector;
        }
    }
}
