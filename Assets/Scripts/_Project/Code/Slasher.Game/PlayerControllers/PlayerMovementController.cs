using Assets.Scripts._Project.Code.Slasher.Game.Constants;
using Assets.Scripts._Project.Code.Slasher.Game.Helpers;
using Assets.Scripts._Project.Code.Slasher.Game.ICharecteristic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _moveSpeed = 3f;

    [Header("Unit references")]
    [SerializeField] private Rigidbody2D _rb;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private UnitCharecteristic _unitCharecteristic;
    [SerializeField] private Animator _animator;

    private Vector2 _lastInputDirection;
    private InputAction _moveAction;

    private void OnEnable()
    {
        if (_playerInput == null)
        {
            return;
        }

        _moveAction = _playerInput.actions[PlayerInputActionNames.MOVE];
        _moveAction.performed += OnMove;
        _moveAction.canceled += OnMove;
    }

    private void OnDisable()
    {
        // при выгрузке сцены PlayerInput может быть уничтожен раньше нас,
        // поэтому отписываемся через сохранённый action, а не через PlayerInput
        if (_moveAction != null)
        {
            _moveAction.performed -= OnMove;
            _moveAction.canceled -= OnMove;
            _moveAction = null;
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (!_unitCharecteristic.IsAlive)
        {
            return;
        }

        var moveInput = context.ReadValue<Vector2>().normalized;
        SetUnitVector(moveInput);
        _lastInputDirection = moveInput;
    }

    private void SetUnitVector(Vector2 vector)
    {
        UnitDirectionHelper.SetDirection(_rb.gameObject, vector);
        UnitAnimationHelper.SetAnimation(_animator, vector);
        _rb.linearVelocity = vector * _moveSpeed;
    }

    public void RestoreMovement()
    {
        if (!_unitCharecteristic.IsAlive)
        {
            return;
        }

        SetUnitVector(_lastInputDirection);
    }
}
