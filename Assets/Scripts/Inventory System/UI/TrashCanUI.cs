using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCanUI : MonoBehaviour, IDropHandler
{
    
    public void OnDrop(PointerEventData eventData)
    {
        if (InventorySlotUI.HasActiveDrag())
        {
            InventorySlotUI.DeleteDraggedItem();
            Debug.Log("Item deleted from inventory or quick bar.");
        }
    }
}
