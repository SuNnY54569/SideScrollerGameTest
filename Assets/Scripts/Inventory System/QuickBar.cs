using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickBar : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private PlayerHandDisplay playerHandDisplay;
    [SerializeField] private int quickBarSize = 8;

    public int QuickBarSize => quickBarSize;
    public int SelectedIndex { get; private set; } = 0;

    public InventorySlot GetSlot(int index)
    {
        if (index >= 0 && index < quickBarSize && index < playerInventory.slots.Count)
        {
            return playerInventory.slots[index];
        }
        return null;
    }

    public InventorySlot GetSelectedSlot()
    {
        return GetSlot(SelectedIndex);
    }

    public void SetSelectedIndex(int index)
    {
        if (index >= 0 && index < quickBarSize)
        {
            SelectedIndex = index;
            var selectedSlot = GetSelectedSlot();
            playerHandDisplay?.DisplayItem(selectedSlot?.item);
        }
        
    }
}
