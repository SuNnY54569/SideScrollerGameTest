using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int totalSlotCount = 20;
    
    public List<InventorySlot> slots = new();

    private void Awake()
    {
        for (int i = 0; i < totalSlotCount; i++)
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
