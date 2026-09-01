using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private PlayerControls controls;
    private Vector2 currentInput;
    private Coroutine moveCoroutine;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnEnable() => controls.Enable();

    private void OnDisable()
    {
        controls.Disable();
        StopMoveLoop();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        currentInput = context.ReadValue<Vector2>();

        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(MoveLoop());
        }
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        currentInput = Vector2.zero;
        StopMoveLoop();
    }

    private IEnumerator MoveLoop()
    {
        while (currentInput != Vector2.zero)
        {
            Vector3 moveDirection = new Vector3(currentInput.x, currentInput.y, 0f);
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);
            yield return null;
        }

        moveCoroutine = null;
    }

    private void StopMoveLoop()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
    }
}
