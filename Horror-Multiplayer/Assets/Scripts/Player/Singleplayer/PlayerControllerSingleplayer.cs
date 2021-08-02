using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerSingleplayer : MonoBehaviour
{
    [SerializeField] private InputActionAsset actions;
    [SerializeField] public CharacterController characterController;
    [SerializeField] Animator anim;

    [SerializeField] public float movementSpeed = 3;
    [SerializeField] public float sprintSpeed = 6;
    [SerializeField] float gravity = -30f; // -9.81
    [SerializeField] float jumpHeight = 3;

    [SerializeField] Vector2 movementInput;
    Vector3 velocity;

    [SerializeField] private bool isSprint;
    [SerializeField] bool isGrounded;
    [SerializeField] bool jump;

    public GameObject Hand;

    private InventoryItemBase mCurrentItem = null;

    public Inventory inventory;
    public void Start()
    {
        var rebinds = PlayerPrefs.GetString("rebinds");
        if (!string.IsNullOrEmpty(rebinds))
            actions.LoadBindingOverridesFromJson(rebinds);

        characterController = GetComponent<CharacterController>();

        var renderColor = GetComponentInChildren<Renderer>();
        renderColor.material.SetColor("_Color", Color.blue);

        //inventory.ItemUsed += Inventory_ItemUsed;
        inventory.ItemRemoved += Inventory_ItemRemoved;
    }

    private void FixedUpdate()
    {
        //CheckPlayer
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //Movement
        Move(movementInput);

        //Gravity
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }
    public void SprintStart()
    {
        isSprint = true;
    }
    public void SprintFinish()
    {
        isSprint = false;
    }
    void Move(Vector2 direction)
    {
        if (isSprint)
        {
            Vector3 moveVector = (transform.right * direction.x + transform.forward * direction.y) * sprintSpeed;
            characterController.Move(moveVector * Time.deltaTime);
        }
        else
        {
            Vector3 moveVector = (transform.right * direction.x + transform.forward * direction.y) * movementSpeed;
            characterController.Move(moveVector * Time.deltaTime);
        }

        if (movementInput == Vector2.zero)
        {
            //Idle
            anim.SetFloat("speed", 0);
        }
        else
        {
            //Moving
            anim.SetFloat("speed", 1);
        }
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) { return; }
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void DropCurrentItem(InputAction.CallbackContext context)
    {
        if (context.performed)
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
    }

    public void DoDropItem()
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