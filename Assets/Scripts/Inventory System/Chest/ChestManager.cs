using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance { get; private set; }

    private Chest currentChest;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void OpenChest(Chest chest)
    {
        currentChest = chest;
        InventoryUI inventoryUI = FindObjectOfType<InventoryUI>();
        if (inventoryUI != null)
        {
            inventoryUI.ToggleInventory();
        }
    }

    public void CloseChest()
    {
        currentChest = null;
    }

    public Chest GetCurrentChest()
    {
        return currentChest;
    }

    public bool IsChestOpen()
    {
        return currentChest != null;
    }
}
