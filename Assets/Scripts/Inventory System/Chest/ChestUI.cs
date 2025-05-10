using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private GameObject panel;

    private List<InventorySlotUI> slotUIs = new();
    private Inventory chestInventory;

    public void Open(Inventory inventory)
    {
        chestInventory = inventory;
        panel.SetActive(true);
        RefreshUI();
    }

    public void Close()
    {
        ClearUI();
        panel.SetActive(false);
    }

    private void RefreshUI()
    {
        ClearUI();
        for (int i = 0; i < chestInventory.slots.Count; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(chestInventory, null, i);
            slotUIs.Add(slotUI);
        }
    }

    private void ClearUI()
    {
        foreach (var slotUI in slotUIs)
        {
            Destroy(slotUI.gameObject);
        }
        slotUIs.Clear();
    }
}
