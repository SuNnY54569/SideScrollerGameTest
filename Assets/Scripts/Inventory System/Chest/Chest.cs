using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private int chestSlotCount = 16;
    public Inventory chestInventory;

    private void Awake()
    {
        chestInventory = new Inventory(chestSlotCount);
    }
}
