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
        playerInventoryUI?.OpenInventory();
        chestUI?.Open(chest);
    }

    public void CloseChest()
    {
        playerInventoryUI?.CloseInventory();
        chestUI?.Close();
    }
}
