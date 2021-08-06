using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllerSinglePlayer : MonoBehaviour
{
    [SerializeField] float sensitivityX = 20f;
    [SerializeField] float sensitivityY = 0.3f;
    Vector2 mouseInput;

    [Space(10f)]
    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 60f;
    float XRotation = 0f;

    [Space(10f)]
    public LayerMask avoidPlayer;
    private float objectPickupRange = 3;

    #region StartUpdate

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        MoveCamera(mouseInput);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    #endregion

    #region CameraInput

    void MoveCamera(Vector2 mouse)
    {
        XRotation -= mouse.y * sensitivityY;
        XRotation = Mathf.Clamp(XRotation, -xClamp, xClamp);

        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = XRotation;

        playerCamera.eulerAngles = targetRotation;

        transform.Rotate(Vector3.up, mouse.x * sensitivityX * Time.deltaTime);
    }

    public void MouseInput(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    #endregion

    #region Actions

    public void RaycastPickup(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit hit;

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, objectPickupRange, avoidPlayer))
            {
                InventoryItemBase item = hit.collider.GetComponent<InventoryItemBase>();

                if (item != null)
                {
                    //Inventory.instance.AddItem(item);
                    //item.OnPickupServerRpc(NetworkManager.Singleton.LocalClientId);
                    Debug.Log("Nu merge!");
                }
            }
        }
    }

    #endregion
}
