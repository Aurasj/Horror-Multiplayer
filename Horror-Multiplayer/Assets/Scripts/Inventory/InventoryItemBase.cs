using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class InventoryItemBase : NetworkBehaviour
{
    public InventorySlot Slot
    {
        get; set;
    }

    public virtual string Name
    {
        get
        {
            return "base item";
        }
    }

    public Sprite _Image;

    public Sprite Image
    {
        get
        {
            return _Image;
        }
    }
    public virtual void OnUse()
    {
        transform.localPosition = PickPosition;
        transform.localEulerAngles = PickRotation;
    }
    public virtual void OnDrop()
    {
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(true);
            gameObject.transform.position = hit.point;
            gameObject.transform.eulerAngles = DropRotation;
        }
    }
    [ServerRpc]
    public virtual void OnPickupServerRpc()
    {
        Destroy(gameObject.GetComponent<Rigidbody>());
        gameObject.SetActive(false);
    }
    public Vector3 PickPosition;

    public Vector3 PickRotation;

    public Vector3 DropRotation;
}
