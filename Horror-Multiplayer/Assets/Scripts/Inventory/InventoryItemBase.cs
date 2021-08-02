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

    #region OnUse

    public virtual void OnUse()
    {
        OnUserServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void OnUserServerRpc()
    {
        //Debug.Log("Client wants to change object position in the hand parent transform");
        OnUseClientClientRpc();
    }

    [ClientRpc]
    private void OnUseClientClientRpc()
    {
        //Debug.Log("Client changed object position in the hand parent transform");
        transform.localPosition = PickPosition;
        transform.localEulerAngles = PickRotation;
    }

    #endregion

    #region OnDrop

    public virtual void OnDrop()
    {
        //Debug.log("Drop item by drag out inventory item");
        RaycastHit hit = new RaycastHit();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000))
        {
            gameObject.SetActive(true);
            gameObject.transform.position = hit.point;
            gameObject.transform.eulerAngles = DropRotation;
        }
    }

    #endregion

    #region OnPickup

    [ServerRpc(RequireOwnership = false)]
    public virtual void OnPickupServerRpc(ulong netId)
    {
        //Debug.Log("Client wants to pickup object");

        gameObject.GetComponent<NetworkObject>().ChangeOwnership(netId);
        ulong itemNetId = gameObject.GetComponent<NetworkObject>().NetworkObjectId;

        OnPickupClientRpc(itemNetId);
    }

    [ClientRpc]
    private void OnPickupClientRpc(ulong itemNetId)
    {
        //Debug.Log("Client is pickup");
        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[itemNetId];

        Debug.Log(netObj.IsOwner + " aici ");

        Destroy(netObj.gameObject.GetComponent<Collider>());
        Destroy(netObj.gameObject.GetComponent<Rigidbody>());
        netObj.gameObject.SetActive(false);

        //change location

    }

    #endregion

    public Vector3 PickPosition;

    public Vector3 PickRotation;

    public Vector3 DropRotation;
}
