using Assets.Scripts;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float moveSpeed = 3f;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private UnitCharecteristic unitCharecteristic;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        if (unitCharecteristic == null) unitCharecteristic = GetComponent<UnitCharecteristic>();
    }

    private void OnEnable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed += OnMove;
            playerInput.actions["Move"].canceled += OnMove;
        }
    }

    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Move"].performed -= OnMove;
            playerInput.actions["Move"].canceled -= OnMove;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        var moveInput = context.ReadValue<Vector2>();
        UnitDirectionHelper.SetDirection(gameObject, moveInput);
        UnitAnimationHelper.SetAnimation(GetComponentInChildren<Animator>(), moveInput);
        rb.linearVelocity = moveInput * moveSpeed;
    }
}