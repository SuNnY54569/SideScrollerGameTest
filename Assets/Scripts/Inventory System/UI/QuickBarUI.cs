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
        for (int i = 0; i < quickBar.quickSlots.Count; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(null, quickBar, i, true);
            slotUIs.Add(slotUI);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < quickBar.quickSlots.Count; i++)
        {
            var slot = quickBar.quickSlots[i];
            slotUIs[i].Set(slot.item, slot.amount);
            
            slotUIs[i].SetSelected(i == quickBar.SelectedIndex);
        }

        
        for (int i = 0; i < quickBar.quickSlots.Count; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                quickBar.SetSelectedIndex(i);
            }
        }
    }
}
