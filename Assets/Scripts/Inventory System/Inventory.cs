using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int totalSlotCount = 20;
    
    public List<InventorySlot> slots = new();
    
    public UnityEvent OnInventoryChanged = new UnityEvent();

    private void Awake()
    {
        for (int i = 0; i < totalSlotCount; i++)
        {
            slots.Add(new InventorySlot());
        }
    }
    
    public bool TryMergeOrAddItem(InventoryItem item, int quantity = 1)
    {
        int remaining = quantity;

        // Step 1: Try merging into existing stacks
        foreach (var slot in slots)
        {
            if (!slot.IsEmpty && slot.item == item && item.isStackable)
            {
                int spaceLeft = item.maxStack - slot.amount;
                int addAmount = Mathf.Min(spaceLeft, remaining);
                if (addAmount > 0)
                {
                    slot.AddItem(item, addAmount);
                    remaining -= addAmount;
                    NotifyChange();
                }

                if (remaining <= 0)
                    return true;
            }
        }

        // Step 2: Add to empty slots if any remaining
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                int addAmount = Mathf.Min(item.maxStack, remaining);
                slot.AddItem(item, addAmount);
                remaining -= addAmount;
                NotifyChange();

                if (remaining <= 0)
                    return true;
            }
        }

        // Return true if at least some were added, even if not all
        return remaining < quantity;
    }
    
    public int GetItemCount(InventoryItem targetItem)
    {
        int total = 0;
        foreach (var slot in slots)
        {
            if (slot.item == targetItem)
            {
                total += slot.amount;
            }
        }
        return total;
    }
    
    public void RemoveItem(InventoryItem targetItem, int amount)
    {
        for (int i = 0; i < slots.Count && amount > 0; i++)
        {
            if (slots[i].item == targetItem)
            {
                int removed = Mathf.Min(slots[i].amount, amount);
                slots[i].amount -= removed;
                amount -= removed;

                if (slots[i].amount <= 0)
                    slots[i].Clear();
            }
        }
        
        NotifyChange();
    }
    
    public void NotifyChange()
    {
        OnInventoryChanged.Invoke();
    }
}
