using UnityEngine;
using UnityEngine.InputSystem;
using MLAPI;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private PlayerInput playerInput = null;

    Vector2 moveDirection;
    public float moveSpeed = 2;

    public PlayerInput PlayerInput => playerInput;
    public override void NetworkStart()
    {
        base.NetworkStart();

        if (IsLocalPlayer)
        {
            var renderColor = GetComponent<Renderer>();
            renderColor.material.SetColor("_Color", Color.blue);
        }
        else
        {
            var renderColor = GetComponent<Renderer>();
            renderColor.material.SetColor("_Color", Color.red);
        }
    }
    private void FixedUpdate()
    {
        if (!IsLocalPlayer)
        {
            return;
        }
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
