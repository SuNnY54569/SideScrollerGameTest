using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    
    public int slotIndex;
    private Inventory inventory;
    private QuickBar quickBar;
    
    public void Initialize(Inventory inv, QuickBar quick, int index)
    {
        inventory = inv;
        quickBar = quick;
        slotIndex = index;
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
        var slot = inventory.slots[slotIndex];
        if (!slot.IsEmpty)
        {
            string count = slot.item.maxStack > 1 ? slot.amount.ToString() : "";
            DragDropManager.Instance.StartDrag(inventory, slotIndex, slot.item.icon, count);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragDropManager.Instance.EndDrag();
    }

    public void OnDrop(PointerEventData eventData)
    {
        Inventory sourceInv = DragDropManager.Instance.sourceInventory;
        int sourceIndex = DragDropManager.Instance.sourceSlotIndex;

        if (sourceInv != null && sourceInv != inventory)
        {
            // Assign dragged inventory slot to this quick bar slot
            if (quickBar != null)
            {
                quickBar.AssignSlot(slotIndex, sourceIndex); // drag from backpack → quick bar
            }
        }
    }
}
