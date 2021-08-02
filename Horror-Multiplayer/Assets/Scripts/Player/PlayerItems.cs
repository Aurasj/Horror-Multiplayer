using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerItems : NetworkBehaviour
{
    [SerializeField] private Transform hand;
    private GameObject currentItem;

    private InventoryItemBase mCurrentItem = null;

    public override void NetworkStart()
    {
        base.NetworkStart();

        Invoke("AfterStart", 0.5f);
    }

    private void AfterStart()
    {
        Inventory.instance.ItemUsed += Inventory_ItemUsed;
        Inventory.instance.ItemRemoved += Inventory_ItemRemoved;
    }

    #region DropItem

    public void DropCurrent(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!IsLocalPlayer) { return; }

            DropCurrentServerRpc();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void DropCurrentServerRpc()
    {
        //Debug.Log("Client wats to drop the object");
        if (currentItem)
        {
            currentItem.GetComponent<NetworkObject>().RemoveOwnership();

            DropCurrentClientRpc();
        }
    }

    [ClientRpc]
    private void DropCurrentClientRpc()
    {
        //Debug.Log("Client drop the object"); ;

        //TODO animation

        Inventory.instance.RemoveItem(mCurrentItem);

        currentItem.SetActive(true);
        currentItem.transform.parent = null;

        BoxCollider boxCol = currentItem.AddComponent<BoxCollider>();
        Rigidbody rbItem = currentItem.AddComponent<Rigidbody>();
        if (rbItem != null)
        {
            rbItem.AddForce(transform.forward * 10f, ForceMode.Impulse);

            Invoke("DoDropItemClientRpc", 0.25f);
        }

        currentItem = null;
    }

    [ClientRpc]
    public void DoDropItemClientRpc()
    {
        if (mCurrentItem != null)
        {
            //Debug.Log("Object-rb destroyed");
            Destroy((mCurrentItem as MonoBehaviour).GetComponent<Rigidbody>());

            Debug.Log("ceva");


            mCurrentItem = null;
        }
    }

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        currentItem = (item as MonoBehaviour).gameObject;

        if (item == mCurrentItem)
            mCurrentItem = null;
    }

    #endregion

    #region SetItemActive

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (!IsLocalPlayer) { return; }

        Debug.Log("pickup");

        if (mCurrentItem != null)
        {
            SetItemActive(mCurrentItem, false);
        }

        InventoryItemBase item = e.Item;

        SetItemActive(item, true);

        mCurrentItem = e.Item;
    }

    private void SetItemActive(InventoryItemBase item, bool active)
    {
        ulong itemNetId = item.gameObject.GetComponent<NetworkObject>().NetworkObjectId;

        SetItemActiveServerRpc(itemNetId, active);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetItemActiveServerRpc(ulong itemNetId, bool active)
    {
        //Debug.Log("Client wants to use object");

        SetItemActiveClientRpc(itemNetId, active);

        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[itemNetId];

        currentItem = netObj.gameObject;
    }

    [ClientRpc]
    private void SetItemActiveClientRpc(ulong itemNetId, bool active)
    {
        //Debug.Log("Client is using object");

        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[itemNetId];

        netObj.transform.position = hand.transform.position;
        netObj.transform.SetParent(hand);
        netObj.gameObject.SetActive(active);
    }

    #endregion
}
