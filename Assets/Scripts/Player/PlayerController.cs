using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;

    Vector2 moveDirection;
    public float moveSpeed = 2;

    public PlayerInput PlayerInput => playerInput;
    private void Update()
    {
        Move(moveDirection);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return;}
        Debug.Log("Jump!");
    }
    void Move(Vector2 direction)
    {
        transform.Translate(direction.x * moveSpeed * Time.deltaTime, 0, direction.y * moveSpeed * Time.deltaTime);
    }
}
