//Written By Gabriel Tupy 1-29-2021

using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, ICallAnimateEvents, ICallAudioEvents, ICallVFXEvents
{
    public event Action<string, object> CallAnimationTrigger;
    public event Action<string, int> CallAnimationState;
    public event Action<string, float> CallAudio;
    public event Action<Vector2, Quaternion, float> CallVFX;

    [SerializeField] private InputController input = null;
    [SerializeField] private CharacterType character = null;
    private Rigidbody2D motor = null;
    private Joint2D joint = null;
    [SerializeField] private LayerMask interactionLayer;
    private Vector2 lookDirection = Vector2.zero;
    private Vector2 currentInput = Vector2.zero;
    private float resetDashTimer = 0f;
    private float dashTimer = 0f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float currentMoveSpeed = 0f;
    [SerializeField] private float currentDashLength = 0f;
    [SerializeField] private float currentDashResetLength = 0f;
    [SerializeField] private float currentDashSpeed = 0f;
    private bool canDash = true;
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
            currentDashResetLength = character.GetDashResetLength();
        }
    }

    private void Update()
    {
        if (input == null)
        {
            input = FindObjectOfType<InputController>();
            SubscribeToInput(input);
        }

        if (character != null)
        {
            Debug.DrawRay(this.transform.position, lookDirection.normalized * character.GetInteractionRange(), Color.red);
        }
        if (dashTimer <= 0f)
        {
            currentSpeed = currentMoveSpeed;
            isDashing = false;
        }
        else
        {
            isDashing = true;
            currentSpeed = currentDashSpeed;
            dashTimer -= Time.deltaTime;
        }

        if (resetDashTimer <= 0)
        {
            canDash = true;
        }
        else
        {
            resetDashTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            motor.MovePosition(motor.position + (lookDirection.normalized * currentSpeed * Time.deltaTime));
        }
        else
        {
            motor.MovePosition(motor.position + (currentInput.normalized * currentSpeed * Time.deltaTime));
        }
        DetermineAnimationCall();
        DetermineAudioCall();
    }

    private void DetermineAnimationCall()
    {
        if (currentInput == Vector2.zero)
        {
            if (lookDirection.y > 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationState?.Invoke("IdleBackwards", 0);
            }
            if (lookDirection.y < 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationState?.Invoke("IdleForward", 0);
            }
            if (lookDirection.x < 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationState?.Invoke("IdleLeft", 0);
            }
            if (lookDirection.x > 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationState?.Invoke("IdleRight", 0);
            }
        }
        else
        {
            if (lookDirection.y > 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationState?.Invoke("WalkingBackwards", 0);
            }
            if (lookDirection.y < 0 && lookDirection.x < .5 && lookDirection.x > -.5)
            {
                CallAnimationState?.Invoke("WalkingForward", 0);
            }
            if (lookDirection.x < 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationState?.Invoke("WalkingLeft", 0);
            }
            if (lookDirection.x > 0 && lookDirection.y < .5 && lookDirection.y > -.5)
            {
                CallAnimationState?.Invoke("WalkingRight", 0);
            }
        }
    }

    private void DetermineAudioCall()
    {
        if (isDashing)
        {
            CallAudio?.Invoke("Dash", 0f);
            CallVFX?.Invoke(this.transform.position - new Vector3(0, 1, 0), Quaternion.identity, .1f);
        }
        else
        {
            if (currentInput != Vector2.zero)
            {
                CallAudio?.Invoke("Walking", .1f);
            }
        }
    }

    public void OnMovement(Vector2 direction)
    {
        currentInput = direction;
        if (direction != Vector2.zero && !isDashing)
        {
            lookDirection = direction;
        }
    }

    public void OnGrab()
    {
        if (character == null)
        {
            return;
        }
        RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, lookDirection.normalized, character.GetInteractionRange(), interactionLayer);
        if (hitInfo)
        {
            IGrabbable item = hitInfo.transform.gameObject.GetComponentInParent<IGrabbable>();
            if (item != null)
            {
                //currentGrabItem = item.Grab(this.gameObject);
                currentGrabItem = item.Grab(motor, hitInfo.point);
                CallAnimationTrigger?.Invoke("", null);
                CallAudio?.Invoke("Grab", 0f);
            }
        }
    }

    public void OnRelease()
    {
        if (currentGrabItem != null)
        {
            //currentGrabItem.Release(this.gameObject);
            currentGrabItem.Release(currentInput);
            currentGrabItem = null;
            CallAnimationTrigger?.Invoke("", null);
            CallAudio?.Invoke("Release", 0f);
        }
    }

    public void OnDash()
    {
        if (canDash)
        {
            Debug.Log("I am being called.");
            dashTimer = currentDashLength;
            resetDashTimer = currentDashResetLength;
            canDash = false;
            CallAnimationTrigger?.Invoke("", null);
        }
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
