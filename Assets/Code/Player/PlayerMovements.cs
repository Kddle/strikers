using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    PlayerControls _controls;
    public PlayerControls GetControls => _controls;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    CapsuleCollider2D capsuleCollider;

    float movementInput;

    // PUBLIC
    public float WalkSpeed = 10f;
    public float JumpForce = 20f;

    public float FallMultiplier = 3f;
    public float MovementSmoothing = 0.05f;

    public bool isGrounded;
    public LayerMask GroundLayers;

    public int JumpStack = 1;

    Animator animator;
    Vector2 refVelocity = Vector2.zero;
    int remainingJump;
    bool shouldJump;
    bool isFalling = false;

    public bool IsFacingRight;

    private void Awake()
    {
        spriteRenderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = transform.Find("Renderer").GetComponent<Animator>();

        _controls = new PlayerControls();
        _controls.Gameplay.Move.performed += PerformMovement;
        _controls.Gameplay.Move.canceled += StopMovement;

        _controls.Gameplay.Jump.started += PerformJump;

        _controls.Gameplay.Crouch.performed += StartCrouch;
        _controls.Gameplay.Crouch.canceled += StopCrouch;

        remainingJump = JumpStack;
    }

    private void PerformJump(InputAction.CallbackContext obj)
    {
        if (remainingJump > 0)
        {
            shouldJump = true;
            isFalling = true;
            animator.SetBool("isFalling", true);
        }
    }

    private void StopCrouch(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void StartCrouch(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void StopMovement(InputAction.CallbackContext obj)
    {
        movementInput = 0f;
    }

    private void PerformMovement(InputAction.CallbackContext obj)
    {
        var input = obj.ReadValue<Vector2>();

        if (input.x == 1f)
            IsFacingRight = true;
        else if (input.x == -1f)
            IsFacingRight = false;

        movementInput = input.x;
    }

    private void Update()
    {
        isGrounded = capsuleCollider.IsTouchingLayers(GroundLayers);

        if (isGrounded)
        {
            remainingJump = JumpStack;

            if (isFalling)
            {
                animator.SetBool("isFalling", false);
                isFalling = false;
            }
        }
    }

    private void FixedUpdate()
    {
        // Movements
        Vector2 targetVelocity = new Vector3(movementInput * WalkSpeed * Time.fixedDeltaTime * 10f, rb.velocity.y);
        rb.velocity = Vector2.SmoothDamp(rb.velocity, targetVelocity, ref refVelocity, MovementSmoothing);

        animator.SetFloat("Velocity", Mathf.Abs(rb.velocity.x));

        if (isGrounded && shouldJump)
        {
            shouldJump = false;
            rb.AddForce(new Vector2(0f, JumpForce), ForceMode2D.Impulse);
        }
        else if (!isGrounded && shouldJump)
        {
            if (rb.velocity.y >= 0f)
                rb.AddForce(new Vector2(0f, JumpForce * 0.8f), ForceMode2D.Impulse);
            else
                rb.AddForce(new Vector2(0f, JumpForce * 1.3f), ForceMode2D.Impulse);

            shouldJump = false;
            remainingJump -= 1;
        }

        // Extra gravity
        if (rb.velocity.y < 0f)
        {
            isFalling = true;
            rb.velocity += Vector2.down * FallMultiplier * Time.fixedDeltaTime;
        }
    }

    private void LateUpdate()
    {
        spriteRenderer.flipX = !IsFacingRight;
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}
