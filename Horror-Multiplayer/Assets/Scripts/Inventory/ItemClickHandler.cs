using UnityEngine;
using UnityEngine.UI;

public class ItemClickHandler : MonoBehaviour
{
    public Inventory inventory;

    public KeyCode key;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
    }
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            FadeToColor(button.colors.pressedColor);

            button.onClick.Invoke();
        }
        else if (Input.GetKeyUp(key))
        {
            FadeToColor(button.colors.normalColor);
        }
    }

    void FadeToColor(Color color)
    {
        Graphic graphic = GetComponent<Graphic>();
        graphic.CrossFadeColor(color, button.colors.fadeDuration, true, true);
    }

    private InventoryItemBase AttachedItem
    {
        get
        {
            ItemDragHandler dragHandler =
            gameObject.transform.Find("ItemImage").GetComponent<ItemDragHandler>();

            return dragHandler.Item;
        }
    }
    public void OnItemClicked()
    {
        InventoryItemBase item = AttachedItem;

        if (item != null)
        {
            inventory.UseItem(item);
        }
    }
}
