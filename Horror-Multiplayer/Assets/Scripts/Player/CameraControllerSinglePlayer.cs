using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllerSinglePlayer : MonoBehaviour
{
    [SerializeField] float sensitivityX = 8f;
    [SerializeField] float sensitivityY = 0.5f;
    Vector2 mouseInput;

    [SerializeField] Transform playerCamera;
    [SerializeField] float xClamp = 85f;
    float XRotation = 0f;

    Inventory inventory;
    public LayerMask avoidPlayer;
    private float objectPickupRange = 3;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        AudioListener[] audioListener = FindObjectsOfType<AudioListener>();
        for (int i = 0; i < audioListener.Length; i++)
        {

            DestroyImmediate(audioListener[i]);
        }

        inventory = FindObjectOfType<Inventory>();
    }

    private void FixedUpdate()
    {
        MoveCamera(mouseInput);
    }
    void MoveCamera(Vector2 mouse)
    {
        XRotation -= mouse.y * sensitivityY;
        XRotation = Mathf.Clamp(XRotation, -xClamp, xClamp);
        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = XRotation;
        playerCamera.eulerAngles = targetRotation;

        transform.Rotate(Vector3.up, mouse.x * sensitivityX * Time.deltaTime);

        /*float mouseX = mouse.x * sensitivityX * Time.deltaTime;
        float mouseY = mouse.y * sensitivityY * Time.deltaTime;

        XRotation -= mouseY;

        XRotation = Mathf.Clamp(XRotation, -xClamp, xClamp);

        playerCamera.transform.localRotation = Quaternion.Euler(XRotation, 0, 0);

        transform.Rotate(Vector3.up * mouseX * 3);*/
    }
    public void MouseInput(InputAction.CallbackContext context)
    {
        mouseInput = context.ReadValue<Vector2>();
    }

    public void RaycastPickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, objectPickupRange, avoidPlayer))
        {
            InventoryItemBase item = hit.collider.GetComponent<InventoryItemBase>();

            if(item != null)
            {
                inventory.AddItem(item);
                item.OnPickupServerRpc();
            }
        }
    }
}
