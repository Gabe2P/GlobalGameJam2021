//Written By Gabriel Tupy 1-29-2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private InputController input = null;
    [SerializeField] private CharacterType character = null;
    private Rigidbody2D motor = null;
    private Joint2D joint = null;
    [SerializeField] private LayerMask interactionLayer;
    private Vector2 currentInput = Vector2.zero;
    private float timer = 0f;
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float currentMoveSpeed = 0f;
    [SerializeField] private float currentDashLength = 0f;
    [SerializeField] private float currentDashSpeed = 0f;
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
            Debug.DrawRay(this.transform.position, this.transform.position + new Vector3(currentInput.x, currentInput.y, this.transform.position.z) * character.GetInteractionRange(), Color.red);
        }
        if (timer <= 0f)
        {
            currentSpeed = currentMoveSpeed;
        }
        else
        {
            currentSpeed = currentDashSpeed;
            timer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        motor.AddForce((currentInput) * currentSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }

    public void OnMovement(Vector2 direction)
    {
        Debug.Log("I am being called.");
        currentInput = direction;
    }

    public void OnGrab()
    {
        Debug.Log("I am being called.");
        if (character == null)
        {
            return;
        }
        RaycastHit2D hitInfo = Physics2D.Raycast(this.transform.position, this.transform.position + new Vector3(currentInput.x, currentInput.y, this.transform.position.z), character.GetInteractionRange(), interactionLayer);
        if (hitInfo)
        {
            IGrabbable item = hitInfo.transform.gameObject.GetComponent<IGrabbable>();
            if (item != null)
            {
                currentGrabItem = item.Grab(this.gameObject);
            }
        }
    }

    public void OnRelease()
    {
        if (currentGrabItem != null)
        {
            currentGrabItem.Release(this.gameObject);
        }
    }

    public void OnDash()
    {
        Debug.Log("I am being called.");
        timer = currentDashLength;
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
