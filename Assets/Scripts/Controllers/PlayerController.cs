//Written By Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, ICallAnimateEvents
{
    public event Action<string, object> CallAnimationTrigger;
    public event Action<string, int> CallAnimationState;

    [SerializeField] private InputController input = null;
    [SerializeField] private CharacterType character = null;
    private Rigidbody2D motor = null;
    private Joint2D joint = null;
    [SerializeField] private LayerMask interactionLayer;
    private Vector2 lookDirection = Vector2.zero;
    private Vector2 currentInput = Vector2.zero;
    private float timer = 0f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float currentMoveSpeed = 0f;
    [SerializeField] private float currentDashLength = 0f;
    [SerializeField] private float currentDashSpeed = 0f;
    private bool isDashing = false;
    private IGrabbable currentGrabItem = null;

    private void Awake()
    {
        motor = GetComponent<Rigidbody2D>();
        joint = GetComponent<Joint2D>();
        if (character != null)
        {
            currentMoveSpeed = character.GetMoveSpeed();
            currentDashSpeed = character.GetDashSpeed();
            currentDashLength = character.GetDashLength();
        }
    }

    private void Update()
    {
        if (character != null)
        {
            Debug.DrawRay(this.transform.position, lookDirection.normalized * character.GetInteractionRange(), Color.red);
        }
        if (timer <= 0f)
        {
            currentSpeed = currentMoveSpeed;
            isDashing = false;
        }
        else
        {
            isDashing = true;
            currentSpeed = currentDashSpeed;
            timer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            motor.MovePosition(motor.position + (lookDirection.normalized * currentSpeed * Time.deltaTime));
            DetermineAnimationCall();
        }
        else
        {
            motor.MovePosition(motor.position + (currentInput.normalized * currentSpeed * Time.deltaTime));
            DetermineAnimationCall();
        }
    }

    private void DetermineAnimationCall()
    {
        if (currentInput == Vector2.zero)
        {
            if (lookDirection.y > 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationTrigger?.Invoke("isIdle", null);
            }
            if (lookDirection.y < 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationTrigger?.Invoke("isIdle", null);
            }
            if (lookDirection.x < 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationTrigger?.Invoke("isIdle", null);
            }
            if (lookDirection.x > 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationTrigger?.Invoke("isIdle", null);
            }
        }
        else
        {
            if (lookDirection.y > 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationTrigger?.Invoke("isMovingDown", true);
            }
            if (lookDirection.y < 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationTrigger?.Invoke("isMovingUp", true);
            }
            if (lookDirection.x < 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationTrigger?.Invoke("isMovingLeft", true);
            }
            if (lookDirection.x > 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationTrigger?.Invoke("isMovingRight", true);
            }
        }
    }

    public void OnMovement(Vector2 direction)
    {
        Debug.Log("I am being called.");
        currentInput = direction;
        if (direction != Vector2.zero && !isDashing)
        {
            lookDirection = direction;
        }
    }

    public void OnGrab()
    {
        Debug.Log("I am being called.");
        if (character == null)
        {
            return;
        }
        RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, lookDirection.normalized, character.GetInteractionRange(), interactionLayer);
        if (hitInfo)
        {
            Debug.Log("Found Item.");
            IGrabbable item = hitInfo.transform.gameObject.GetComponentInParent<IGrabbable>();
            if (item != null)
            {
                //currentGrabItem = item.Grab(this.gameObject);
                currentGrabItem = item.Grab(motor, hitInfo.point);
                CallAnimationTrigger?.Invoke("", null);
            }
        }
    }

    public void OnRelease()
    {
        if (currentGrabItem != null)
        {
            //currentGrabItem.Release(this.gameObject);
            currentGrabItem.Release(currentInput);
            CallAnimationTrigger?.Invoke("", null);
        }
    }

    public void OnDash()
    {
        Debug.Log("I am being called.");
        timer = currentDashLength;
        CallAnimationTrigger?.Invoke("", null);
    }

    private void SubscribeToInput(InputController reference)
    {
        if (reference != null)
        {
            reference.OnGrabButtonPress += OnGrab;
            reference.OnGrabButtonRelease += OnRelease;
            reference.OnDashButtonPress += OnDash;
            reference.OnMovementChanged += OnMovement;
        }
    }

    private void UnsubscribeToInput(InputController reference)
    {
        if (reference != null)
        {
            reference.OnGrabButtonPress -= OnGrab;
            reference.OnGrabButtonRelease -= OnRelease;
            reference.OnDashButtonPress -= OnDash;
            reference.OnMovementChanged -= OnMovement;
        }
    }

    private void OnEnable()
    {
        SubscribeToInput(input);
    }

    private void OnDisable()
    {
        UnsubscribeToInput(input);
    }
}
