using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public Inventory inventory;

    public void OnDrop(PointerEventData eventData)
    {
        RectTransform invPanel = transform as RectTransform;

        if (!RectTransformUtility.RectangleContainsScreenPoint(invPanel, Input.mousePosition))
        {
            IInventoryItem item = eventData.pointerDrag.gameObject.GetComponent<ItemDragHandler>().Item;
            if (item != null)
            {
                inventory.RemoveItem(item);
                item.OnDrop();
            }
        }
    }
}
