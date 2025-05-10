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
    
    private void Update()
    {
        if (currentChest == null) return;

        for (int i = 0; i < currentChest.chestInventory.slots.Count; i++)
        {
            var slot = currentChest.chestInventory.slots[i];
            slotUIs[i].Set(slot.item, slot.amount);
        }
    }

    public void Open(Chest chest)
    {
        ClearUI();
        currentChest = chest;
        chestPanel.SetActive(true);

        PrepareSlots();
        currentChest.chestInventory.OnInventoryChanged.AddListener(UpdateUI);

        InventoryUI.Instance.ForceOpen(); // Ensure player inventory is open too
        
        UpdateUI();
    }

    public void Close()
    {
        if (currentChest != null)
        {
            currentChest.chestInventory.OnInventoryChanged.RemoveListener(UpdateUI);
        }
        
        InventoryUI.Instance.ForceClose();
        chestPanel.SetActive(false);
        currentChest = null;
    }
    
    private void PrepareSlots()
    {
        if (slotUIs.Count == currentChest.chestInventory.slots.Count) return;

        ClearUI();
        for (int i = 0; i < currentChest.chestInventory.slots.Count; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(currentChest.chestInventory, null, i);
            slotUIs.Add(slotUI);
        }
    }
    
    private void UpdateUI()
    {
        // Refresh slotUIs based on current chestInventory
        for (int i = 0; i < slotUIs.Count; i++)
        {
            var slot = currentChest.chestInventory.slots[i];
            slotUIs[i].Set(slot.item, slot.amount);
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
