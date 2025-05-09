using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public InventoryItem item;
    public int amount;

    public bool IsEmpty => item == null || amount <= 0;

    public bool CanAdd(InventoryItem newItem)
    {
        if (IsEmpty) return true;
        return item == newItem && item.isStackable && amount < item.maxStack;
    }

    public void AddItem(InventoryItem newItem, int quantity)
    {
        if (item == null)
        {
            item = newItem;
            amount = quantity;
        }
        else if (item == newItem && item.isStackable)
        {
            amount = Mathf.Min(amount + quantity, item.maxStack);
        }
    }

    public void RemoveItem(int quantity)
    {
        amount -= quantity;
        if (amount <= 0) Clear();
    }

    public void Clear()
    {
        item = null;
        amount = 0;
    }
}
