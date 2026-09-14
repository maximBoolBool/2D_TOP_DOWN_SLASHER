using Assets.Scripts;
using Assets.Scripts.Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 3f;

    [Header("References")]
    private Rigidbody2D _rb;
    private PlayerInput _playerInput;
    private UnitCharecteristic _unitCharecteristic;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _unitCharecteristic = GetComponent<UnitCharecteristic>();
    }

    private void OnEnable()
    {
        if (_playerInput != null)
        {
            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMove;
        }
    }

    private void OnDisable()
    {
        if (_playerInput != null)
        {
            _playerInput.actions["Move"].performed -= OnMove;
            _playerInput.actions["Move"].canceled -= OnMove;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if(!_unitCharecteristic.IsAlive)
        {
            return;
        }

        var moveInput = context.ReadValue<Vector2>();
        UnitDirectionHelper.SetDirection(gameObject, moveInput);
        UnitAnimationHelper.SetAnimation(GetComponent<Animator>(), moveInput);
        _rb.linearVelocity = moveInput * _moveSpeed;
    }
}