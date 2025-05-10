using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int chestSlotCount = 16;
    public Inventory chestInventory { get; private set; }

    private void Awake()
    {
        chestInventory = new Inventory(chestSlotCount);
    }

    public void OpenChest()
    {
        ChestUI.Instance.Open(this);
    }

    public void CloseChest()
    {
        ChestUI.Instance.Close();
    }
}
