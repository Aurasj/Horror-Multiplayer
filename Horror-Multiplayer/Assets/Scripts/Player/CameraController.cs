using MLAPI;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : NetworkBehaviour
{
    [SerializeField] float sensitivityX = 20f;
    [SerializeField] float sensitivityY = 0.3f;
    Vector2 mouseInput;

    [Space(10f)]
    [SerializeField] Transform playerCamera;
    [SerializeField] Transform headPlayer;
    [SerializeField] float xClamp = 60f;
    float XRotation = 0f;

    [Space(10f)]
    public LayerMask avoidPlayer;
    private float objectPickupRange = 3;

    #region StartUpdate

    public override void NetworkStart()
    {
        base.NetworkStart();

        Cursor.lockState = CursorLockMode.Locked;

        if (!IsLocalPlayer)
        {
            playerCamera.GetComponent<AudioListener>().enabled = false;
            playerCamera.GetComponent<Camera>().enabled = false;
        }
        else
        {
            AudioListener[] audioListener = FindObjectsOfType<AudioListener>();
            for (int i = 0; i < audioListener.Length; i++)
            {
                //DestroyImmediate(audioListener[i]);
                audioListener[i].enabled = false;
            }

            playerCamera.GetComponent<AudioListener>().enabled = true;
        }
    }

    private void FixedUpdate()
    {
        if (!IsLocalPlayer) { return; }

        if (InMultiplayerGameManager.instance)
        {
            if (InMultiplayerGameManager.instance.playerOnEnable)
            {
                MoveCamera(mouseInput);
            }
        }
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
                    if (!IsLocalPlayer) { return; }

                    Inventory.instance.AddItem(item);
                    item.OnPickupServerRpc(NetworkManager.Singleton.LocalClientId);
                }
            }
        }
    }

    #endregion

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
