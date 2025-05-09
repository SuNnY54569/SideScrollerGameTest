using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int slotCount = 8;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(new InventorySlot());
        }
    }
    
    public bool AddItem(InventoryItem item, int quantity = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.CanAdd(item))
            {
                slot.AddItem(item, quantity);
                return true;
            }
        }
        
        foreach (var slot in slots)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item, quantity);
                return true;
            }
        }

        // Inventory full
        return false;
    }
    
    public void RemoveItem(InventoryItem item, int quantity = 1)
    {
        foreach (var slot in slots)
        {
            if (slot.item == item)
            {
                slot.RemoveItem(quantity);
                return;
            }
        }
    }
}
