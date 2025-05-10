using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandDisplay : MonoBehaviour
{
    [SerializeField] private Transform handAnchor; // Empty GameObject as placement
    private GameObject currentDisplay;
    private InventoryItem currentItem;

    public void DisplayItem(InventoryItem item)
    {
        if (currentItem == item)
            return;
        
        ClearDisplay();

        if (item != null && item.worldDisplayPrefab != null)
        {
            currentDisplay = Instantiate(item.worldDisplayPrefab, handAnchor);
            currentDisplay.transform.localPosition = Vector3.zero;
            currentDisplay.transform.localRotation = Quaternion.identity;
            currentItem = item; 
        }
        else
        {
            currentItem = null;
        }
    }

    public void ClearDisplay()
    {
        if (currentDisplay != null)
        {
            Destroy(currentDisplay);
            currentDisplay = null;
        }

        currentItem = null;
    }
}
