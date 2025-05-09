using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickBar : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private int quickBarSize = 6;
    
    public List<int> quickBarSlotIndices = new();
    
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
        
        for (int i = 0; i < quickBarSize; i++)
        {
            quickBarSlotIndices.Add(i);
        }
    }
    
    public List<InventorySlot> GetQuickBarSlots()
    {
        List<InventorySlot> slots = new();
        foreach (int index in quickBarSlotIndices)
        {
            slots.Add(inventory.slots[index]);
        }
        return slots;
    }
    
    public void AssignSlot(int quickSlotIndex, int inventorySlotIndex)
    {
        if (quickSlotIndex >= 0 && quickSlotIndex < quickBarSlotIndices.Count &&
            inventorySlotIndex >= 0 && inventorySlotIndex < inventory.slots.Count)
        {
            quickBarSlotIndices[quickSlotIndex] = inventorySlotIndex;
        }
    }
    
    public InventorySlot GetSlot(int quickSlotIndex)
    {
        if (quickSlotIndex < 0 || quickSlotIndex >= quickBarSlotIndices.Count) return null;
        return inventory.slots[quickBarSlotIndices[quickSlotIndex]];
    }
}
