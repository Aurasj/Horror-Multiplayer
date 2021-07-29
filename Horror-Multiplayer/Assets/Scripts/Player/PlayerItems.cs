using MLAPI;
using MLAPI.Messaging;
using MLAPI.Spawning;
using UnityEngine;

public class PlayerItems : NetworkBehaviour
{
    [SerializeField] private GameObject hand;

    private InventoryItemBase mCurrentItem = null;
    Inventory inventory;

    public override void NetworkStart()
    {
        base.NetworkStart();

        inventory = FindObjectOfType<Inventory>();

        inventory.ItemUsed += Inventory_ItemUsed;
        inventory.ItemRemoved += Inventory_ItemRemoved;
    }

    #region DropItem

    [ServerRpc(RequireOwnership = false)]
    public void DropCurrentServerRpc()
    {
        //Debug.Log("Client wats to drop the object");
        if (!IsLocalPlayer) { return; }
        DropCurrentClientRpc();
    }

    [ClientRpc]
    private void DropCurrentClientRpc()
    {
        //Debug.Log("Client drop the object"); ;

        if (mCurrentItem != null)
        {
            //TODO animation

            GameObject goItem = (mCurrentItem as MonoBehaviour).gameObject;

            inventory.RemoveItem(mCurrentItem);

            goItem.SetActive(true);
            goItem.transform.parent = null;
            goItem.GetComponent<NetworkObject>().RemoveOwnership();

            Rigidbody rbItem = goItem.AddComponent<Rigidbody>();
            if (rbItem != null)
            {
                rbItem.AddForce(transform.forward * 10f, ForceMode.Impulse);

                Invoke("DoDropItemClientRpc", 0.25f);
            }
        }
    }

    [ClientRpc]
    public void DoDropItemClientRpc()
    {
        if (mCurrentItem != null)
        {
            //Debug.Log("Object-rb destroyed");
            Destroy((mCurrentItem as MonoBehaviour).GetComponent<Rigidbody>());

            mCurrentItem = null;
        }
    }

    private void Inventory_ItemRemoved(object sender, InventoryEventArgs e)
    {
        InventoryItemBase item = e.Item;

        GameObject goItem = (item as MonoBehaviour).gameObject;
        // goItem.SetActive(true);
        //goItem.transform.parent = null;

        if (item == mCurrentItem)
            mCurrentItem = null;

        Debug.Log(goItem.transform.parent);
    }

    #endregion

    #region SetItemActive

    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (!IsLocalPlayer) { return; }
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
    }

    [ClientRpc]
    private void SetItemActiveClientRpc(ulong itemNetId, bool active)
    {
        //Debug.Log("Client is using object");

        NetworkObject netObj = NetworkSpawnManager.SpawnedObjects[itemNetId];

        //netObj.transform.parent = active ? hand.transform : null;
        netObj.transform.position = hand.transform.position;
        netObj.transform.SetParent(hand.transform);

        netObj.gameObject.SetActive(active);
    }

    #endregion
}
