using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance { get; private set; }

    [SerializeField] private GameObject chestPanel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;

    private List<InventorySlotUI> slotUIs = new();
    private Chest currentChest;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        chestPanel.SetActive(false);
    }

    public void Open(Chest chest)
    {
        ClearUI();
        currentChest = chest;
        chestPanel.SetActive(true);

        foreach (var slot in currentChest.chestInventory.slots)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(currentChest.chestInventory, null, currentChest.chestInventory.slots.IndexOf(slot));
            slotUIs.Add(slotUI);
        }
    }

    public void Close()
    {
        ClearUI();
        chestPanel.SetActive(false);
        currentChest = null;
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
