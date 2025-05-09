using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickBar : MonoBehaviour
{
    [SerializeField] private int quickBarSize = 6;
    public List<InventorySlot> quickSlots = new();
    public int SelectedIndex { get; private set; } = 0;
    
    private void Awake()
    {
        
        for (int i = 0; i < quickBarSize; i++)
        {
            quickSlots.Add(new InventorySlot());
        }
    }
    
    public void AssignSlot(int quickSlotIndex, InventoryItem item, int amount = 1)
    {
        if (quickSlotIndex < 0 || quickSlotIndex >= quickSlots.Count) return;

        quickSlots[quickSlotIndex].item = item;
        quickSlots[quickSlotIndex].amount = Mathf.Clamp(amount, 1, item.maxStack);
    }
    
    public void ClearSlot(int index)
    {
        if (index >= 0 && index < quickSlots.Count)
        {
            quickSlots[index].Clear();
        }
    }
    
    public InventorySlot GetSlot(int index)
    {
        return index >= 0 && index < quickSlots.Count ? quickSlots[index] : null;
    }
    
    public InventorySlot GetSelectedSlot()
    {
        return quickSlots[SelectedIndex];
    }
    
    public void SetSelectedIndex(int index)
    {
        if (index >= 0 && index < quickSlots.Count)
        {
            SelectedIndex = index;
        }
    }
    
    public bool TryAddItem(InventoryItem item, int amount)
    {
        for (int i = 0; i < quickSlots.Count; i++)
        {
            if (quickSlots[i].IsEmpty)
            {
                quickSlots[i].AddItem(item, amount);
                return true;
            }
        }
        return false; // No available slot
    }
}
