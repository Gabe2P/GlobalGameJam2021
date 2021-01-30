//Written By Gabriel Tupy 1-29-2021

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput input = null;
    [SerializeField] private CharacterType character = null;
    private Rigidbody2D motor = null;
    private Vector2 currentInput = Vector2.zero;
    private float currentMoveSpeed = 0f;
    private float currentDashSpeed = 0f;

    private void Awake()
    {
        motor = GetComponent<Rigidbody2D>();

        if (character != null)
        {
            currentMoveSpeed = character.GetMoveSpeed();
            currentMoveSpeed = character.GetDashSpeed();
        }
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        motor.MovePosition(currentInput + motor.position * currentMoveSpeed * Time.deltaTime);
    }

    private void StoreInput(Vector2 direction)
    {
        currentInput = direction;
    }

    private void Grab()
    { 
        
    }

    private void Dash()
    { 
    
    }

    private void SubscribeToInput(PlayerInput reference)
    {
        if (reference != null)
        {
            reference.Gameplay.Movement.performed += ctx => StoreInput(ctx.ReadValue<Vector2>());
            reference.Gameplay.Grab.performed += _ => Grab();
            reference.Gameplay.Dash.performed += _ => Dash();
        }
    }

    private void UnsubscribeToInput(PlayerInput reference)
    {
        if (reference != null)
        {
            reference.Gameplay.Movement.performed -= ctx => StoreInput(ctx.ReadValue<Vector2>());
            reference.Gameplay.Grab.performed -= ctx => Grab();
            reference.Gameplay.Dash.performed -= ctx => Dash();
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
