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
    private Inventory currentChestInventory;
    
    public bool IsOpen => chestPanel.activeSelf;

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
        if (currentChestInventory == null) return;

        for (int i = 0; i < currentChestInventory.slots.Count; i++)
        {
            var slot = currentChestInventory.slots[i];
            slotUIs[i].Set(slot.item, slot.amount);
        }
    }

    public void Open(Inventory chestInventory)
    {
        ClearUI();
        currentChestInventory = chestInventory;
        chestPanel.SetActive(true);

        PrepareSlots();
        currentChestInventory.OnInventoryChanged.AddListener(UpdateUI);

        InventoryUI.Instance.ForceOpen(); // Ensure player inventory is open too
        
        UpdateUI();
    }

    public void Close()
    {
        if (currentChestInventory != null)
        {
            currentChestInventory.OnInventoryChanged.RemoveListener(UpdateUI);
        }
        
        InventoryUI.Instance.ForceClose();
        chestPanel.SetActive(false);
        currentChestInventory = null;
    }
    
    private void PrepareSlots()
    {
        if (slotUIs.Count == currentChestInventory.slots.Count) return;

        ClearUI();
        for (int i = 0; i < currentChestInventory.slots.Count; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(currentChestInventory, null, i);
            slotUIs.Add(slotUI);
        }
    }
    
    private void UpdateUI()
    {
        // Refresh slotUIs based on current chestInventory
        for (int i = 0; i < slotUIs.Count; i++)
        {
            var slot = currentChestInventory.slots[i];
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
