using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject dragIconPrefab;
    
    public int slotIndex;
    private Inventory inventory;
    private QuickBar quickBar;
    private bool isQuickBarSlot;
    
    private GameObject dragIconInstance;
    private static InventoryItem draggedItem;
    private static int draggedAmount;
    
    public void Initialize(Inventory inv, QuickBar quick, int index, bool isQuickBar = false)
    {
        inventory = inv;
        quickBar = quick;
        slotIndex = index;
        this.isQuickBarSlot = isQuickBar;
    }
    
    public void Set(InventoryItem item, int amount)
    {
        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
            countText.text = item.maxStack > 1 ? amount.ToString() : "";
        }
        else
        {
            Clear();
        }
    }

    public void Clear()
    {
        iconImage.sprite = null;
        iconImage.enabled = false;
        countText.text = "";
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventorySlot slot = isQuickBarSlot ? quickBar.quickSlots[slotIndex] : inventory.slots[slotIndex];

        if (slot.IsEmpty) return;
        
        draggedItem = slot.item;
        draggedAmount = slot.amount;
        
        dragIconInstance = Instantiate(dragIconPrefab, transform.root);
        Image icon = dragIconInstance.GetComponentInChildren<Image>();
        TMP_Text count = dragIconInstance.GetComponentInChildren<TMP_Text>();

        icon.sprite = draggedItem.icon;
        count.text = draggedItem.maxStack > 1 ? draggedAmount.ToString() : "";
        dragIconInstance.transform.position = Input.mousePosition;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (dragIconInstance != null)
        {
            dragIconInstance.transform.position = Input.mousePosition;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIconInstance != null)
        {
            Destroy(dragIconInstance);
            dragIconInstance = null;
        }

        draggedItem = null;
        draggedAmount = 0;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem == null) return;

        if (isQuickBarSlot && quickBar != null)
        {
            quickBar.AssignSlot(slotIndex, draggedItem, draggedAmount);
        }
        else if (!isQuickBarSlot && inventory != null)
        {
            inventory.AddItem(draggedItem, draggedAmount);
        }
    }

}
