using MLAPI;
using MLAPI.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private InputActionAsset actions;
    [SerializeField] public CharacterController characterController;
    [SerializeField] Animator anim;

    [Space(20f)]
    [SerializeField] public float movementSpeed = 3;
    [SerializeField] public float sprintSpeed = 6;
    [SerializeField] float gravity = -30f; // -9.81
    [SerializeField] float jumpHeight = 3;

    [Space(20f)]
    [SerializeField] Vector2 movementInput;
    Vector3 velocity;

    [Space(20f)]
    [SerializeField] private bool isSprint;
    [SerializeField] bool isGrounded;

    [SerializeField] public bool isMoving;

    public GameObject Hand;

    private InventoryItemBase mCurrentItem = null;
    Inventory inventory;

    public override void NetworkStart()
    {
        base.NetworkStart();

        if (IsLocalPlayer)
        {
            var renderColor = GetComponentInChildren<Renderer>();
            renderColor.material.SetColor("_Color", Color.blue);

        }
        else
        {
            var renderColor = GetComponentInChildren<Renderer>();
            renderColor.material.SetColor("_Color", Color.red);
        }

        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);

        characterController = GetComponent<CharacterController>();
        inventory = FindObjectOfType<Inventory>();

        inventory.ItemUsed += Inventory_ItemUsed;
        inventory.ItemRemoved += Inventory_ItemRemoved;
    }
    private void Update()
    {
        if (!IsLocalPlayer) { return; }

        //Movement
        Move(movementInput);

        //CheckPlayer
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -0.5f;
        }

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
        if (!Input.GetKeyDown(KeyCode.Space)) { return; }

    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isSprint = true;
        }
        else if (context.canceled)
        {
            isSprint = false;
        }
    }
    void Move(Vector2 direction)
    {
        Vector3 moveVector;

        if (isSprint)
        {
            moveVector = (transform.right * direction.x + transform.forward * direction.y) * sprintSpeed;
            characterController.Move(moveVector * Time.deltaTime);
        }
        else
        {
            moveVector = (transform.right * direction.x + transform.forward * direction.y) * movementSpeed;
            characterController.Move(moveVector * Time.deltaTime);
        }

        if (movementInput == Vector2.zero)
        {
            anim.SetFloat("speed", 0);
            isMoving = false;
        }
        else
        {
            if (isSprint)
            {
                anim.SetFloat("speed", 0.5f);
            }
            else
            {
                anim.SetFloat("speed", 1);
            }
            isMoving = true;

            anim.SetBool("ok", false);
        }
    }
    public void Jump()
    {
        isGrounded = characterController.isGrounded;
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
    [ServerRpc]
    public void DropCurrentServerRpc()
    {
            if (mCurrentItem != null)
            {
                //TODO animation

                GameObject goItem = (mCurrentItem as MonoBehaviour).gameObject;

                inventory.RemoveItem(mCurrentItem);

                Rigidbody rbItem = goItem.AddComponent<Rigidbody>();
                if (rbItem != null)
                {
                    rbItem.AddForce(transform.forward * 2.0f, ForceMode.Impulse);

                    Invoke("DoDropItem", 0.25f);
                }
            }
    }
    [ServerRpc]
    public void DoDropItemServerRpc()
    {
        if (mCurrentItem != null)
        {
            Destroy((mCurrentItem as MonoBehaviour).GetComponent<Rigidbody>());

            mCurrentItem = null;
        }
    }
    private void SetItemActive(InventoryItemBase item, bool active)
    {
        GameObject currentItem = (item as MonoBehaviour).gameObject;
        currentItem.SetActive(active);
        currentItem.transform.parent = active ? Hand.transform : null;
    }
    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (mCurrentItem != null)
        {
            SetItemActive(mCurrentItem, false);
        }

        InventoryItemBase item = e.Item;

        // Use item (put it to hand of the player)
        SetItemActive(item, true);

        mCurrentItem = e.Item;
    }
    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        goItem.SetActive(true);
        goItem.transform.parent = null;

        if (item == mCurrentItem)
            mCurrentItem = null;
    }
}
