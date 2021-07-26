using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
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
        OnUserServerRpc();
    }
    [ServerRpc]
    private void OnUserServerRpc()
    {
        //Debug.Log("Client wants to change object position to hand position");
        OnUseClientClientRpc();
    }
    [ClientRpc]
    private void OnUseClientClientRpc()
    {
        //Debug.Log("Client changed object position to hand position");
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
        //Debug.Log("Client wants to pickup object");

        ulong netId = NetworkManager.Singleton.LocalClientId;
        gameObject.GetComponent<NetworkObject>().ChangeOwnership(netId);
        ulong itemNetId = gameObject.GetComponent<NetworkObject>().NetworkObjectId;

        OnPickupClientRpc(itemNetId);
    }
    [ClientRpc]
    private void OnPickupClientRpc(ulong itemNetId)
    {
        //Debug.Log("Client is pickup");
        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[itemNetId];

        Destroy(netObj.gameObject.GetComponent<Rigidbody>());
        netObj.gameObject.SetActive(false);

    }

    public Vector3 PickPosition;

    public Vector3 PickRotation;

    public Vector3 DropRotation;
}
