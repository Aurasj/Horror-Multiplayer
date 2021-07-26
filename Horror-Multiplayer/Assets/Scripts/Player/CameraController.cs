using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    [SerializeField] float sensitivityX = 20f;
    [SerializeField] float sensitivityY = 0.3f;
    Vector2 mouseInput;

    [SerializeField] Transform playerCamera;
    [SerializeField] Transform headPlayer;
    [SerializeField] float xClamp = 60f;
    float XRotation = 0f;

    InMultiplayerGameManager inMultiplayerGameManager;

    Inventory inventory;
    public LayerMask avoidPlayer;
    private float objectPickupRange = 3;

    public override void NetworkStart()
    {
        base.NetworkStart();

        Cursor.lockState = CursorLockMode.Locked;

        if (!IsLocalPlayer)
        {
            playerCamera.GetComponent<AudioListener>().enabled = false;
            playerCamera.GetComponent<Camera>().enabled = false;
            Debug.Log("!islocal");
        }
        else
        {
            AudioListener[] audioListener = FindObjectsOfType<AudioListener>();
            for (int i = 0; i < audioListener.Length; i++)
            {
                DestroyImmediate(audioListener[i]);
            }
            Debug.Log("islocal");
        }

        inMultiplayerGameManager = FindObjectOfType<InMultiplayerGameManager>();
        inventory = FindObjectOfType<Inventory>();


    }
    private void FixedUpdate()
    {
        if (!IsLocalPlayer) { return; }

        if (inMultiplayerGameManager.playerOnEnable)
        {
            MoveCamera(mouseInput);
        }
    }
    void MoveCamera(Vector2 mouse)
    {
        XRotation -= mouse.y * sensitivityY;
        XRotation = Mathf.Clamp(XRotation, -xClamp, xClamp);

        Vector3 targetRotation = transform.eulerAngles;
        targetRotation.x = XRotation;

        playerCamera.eulerAngles = targetRotation;
        headPlayer.eulerAngles = playerCamera.eulerAngles;

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
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
    public void RaycastPickup()
    {
        RaycastHit hit;

        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, objectPickupRange, avoidPlayer))
        {
            InventoryItemBase item = hit.collider.GetComponent<InventoryItemBase>();

            if (item != null)
            {
                inventory.AddItem(item);
                item.OnPickupServerRpc();
            }
        }
    }

}
