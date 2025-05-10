using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCanUI : MonoBehaviour, IDropHandler
{
    
    [SerializeField] private Transform TargetPoint; // Optional, use trashcan center if null
    [SerializeField] private float Duration = 0.3f;
    
    public void OnDrop(PointerEventData eventData)
    {
        if (InventorySlotUI.HasActiveDrag())
        {
            InventorySlotUI.DeleteDraggedItem();
            Debug.Log("Item deleted from inventory or quick bar.");
        }
    }
}
