using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageContainer : MonoBehaviour
{
    public int capacity = 10;
    public List<InventorySlot> storageSlots = new List<InventorySlot>();
    private void Awake()
    {
        for (int i = 0; i < capacity; i++)
        {
            storageSlots.Add(new InventorySlot());
        }
    }

    public bool AddItem(InventoryItem item, int quantity)
    {
        foreach (var slot in storageSlots)
        {
            if (slot.CanAdd(item))
            {
                slot.AddItem(item, quantity);
                return true;
            }
        }

        foreach (var slot in storageSlots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item, quantity);
                return true;
            }
        }

        return false;
    }
}
