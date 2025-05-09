using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickBarUI : MonoBehaviour
{
    [SerializeField] private QuickBar quickBar;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
    
    private List<InventorySlotUI> slotUIs = new();
    
    private void Start()
    {
        CreateSlots();
        UpdateUI();
    }
    
    private void Update()
    {
        UpdateUI();
    }

    private void CreateSlots()
    {
        var slots = quickBar.GetQuickBarSlots();

        for (int i = 0; i < slots.Count; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            InventorySlotUI ui = slotGO.GetComponent<InventorySlotUI>();
            ui.Initialize(null, null, i); // We don't assign quickBar logic here
            slotUIs.Add(ui);
        }
    }

    private void UpdateUI()
    {
        var slots = quickBar.GetQuickBarSlots();
        for (int i = 0; i < slots.Count; i++)
        {
            slotUIs[i].Set(slots[i].item, slots[i].amount);
        }
    }
}
