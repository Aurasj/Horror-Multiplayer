using MLAPI;
using MLAPI.Messaging;
using UnityEngine;

public class PlayerItems : NetworkBehaviour
{
    [SerializeField] private GameObject hand;

    private GameObject networkObject;
    private bool isSetActive;

    private InventoryItemBase mCurrentItem = null;
    Inventory inventory;

    public override void NetworkStart()
    {
        base.NetworkStart();

        inventory = FindObjectOfType<Inventory>();

        inventory.ItemUsed += Inventory_ItemUsed;
        inventory.ItemRemoved += Inventory_ItemRemoved;
    }

    [ServerRpc]
    public void DropCurrentServerRpc()
    {
        //Debug.Log("Client wats to drop the object");

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

    private void SetItemActive(InventoryItemBase item, bool active)
    {
        networkObject = (item as MonoBehaviour).gameObject;
        isSetActive = active;

        SetItemActiveServerRpc();
    }
    [ServerRpc]
    private void SetItemActiveServerRpc()
    {
        Debug.Log("Client wants to use object");

        SetItemActiveClientRpc();
    }
    [ClientRpc]
    private void SetItemActiveClientRpc()
    {
        Debug.Log("Client is using object");

        networkObject.SetActive(isSetActive);
        networkObject.transform.parent = isSetActive ? hand.transform : null;
    }
    private void Inventory_ItemUsed(object sender, InventoryEventArgs e)
    {
        if (mCurrentItem != null)
        {
            SetItemActive(mCurrentItem, false);
        }

        InventoryItemBase item = e.Item;

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
