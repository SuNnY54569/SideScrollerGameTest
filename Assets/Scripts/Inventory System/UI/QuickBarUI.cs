using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickBarUI : MonoBehaviour
{
    [SerializeField] private QuickBar quickBar;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;
    [SerializeField] private PlayerHandDisplay playerHandDisplay;
    
    private List<InventorySlotUI> slotUIs = new();

    private void Awake()
    {
        if (quickBar == null)
        {
            quickBar = FindObjectOfType<QuickBar>();
        }
    }

    private void Start()
    {
        CreateSlots();
        UpdateUI();
    }
    
    private void Update()
    {
        HandleScrollInput();
        UpdateUI();
    }

    private void CreateSlots()
    {
        for (int i = 0; i < quickBar.QuickBarSize; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer);
            var slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(null, quickBar, i, true);
            slotUIs.Add(slotUI);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < quickBar.QuickBarSize; i++)
        {
            var slot = quickBar.GetSlot(i);
            slotUIs[i].Set(slot?.item, slot?.amount ?? 0);
            slotUIs[i].SetSelected(i == quickBar.SelectedIndex);
        }

        for (int i = 0; i < quickBar.QuickBarSize; i++)
        {
            if (Input.GetKeyDown((KeyCode)((int)KeyCode.Alpha1 + i)))
            {
                quickBar.SetSelectedIndex(i);
            }
        }
        playerHandDisplay.DisplayItem(quickBar.GetSelectedSlot().item);
    }
    
    private void HandleScrollInput()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll > 0f)
        {
            SelectPreviousSlot();
        }
        else if (scroll < 0f)
        {
            SelectNextSlot();
        }
    }
    
    private void SelectNextSlot()
    {
        int nextIndex = (quickBar.SelectedIndex + 1) % quickBar.QuickBarSize;
        quickBar.SetSelectedIndex(nextIndex);
    }

    private void SelectPreviousSlot()
    {
        int prevIndex = (quickBar.SelectedIndex - 1 + quickBar.QuickBarSize) % quickBar.QuickBarSize;
        quickBar.SetSelectedIndex(prevIndex);
    }
}
