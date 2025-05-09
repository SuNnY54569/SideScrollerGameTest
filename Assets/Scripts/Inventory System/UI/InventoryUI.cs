using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private QuickBar quickBar;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotContainer;
    
    private List<InventorySlotUI> slotUIs = new();
    private bool isOpened;
    private CanvasGroup canvasGroup;
    private Tween containerTween;
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
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotContainer.transform);
            InventorySlotUI slotUI = slotGO.GetComponent<InventorySlotUI>();
            slotUI.Initialize(inventory, quickBar, i);
            slotUIs.Add(slotUI);
        }
    }

    private void UpdateUI()
    {
        for (int i = 0; i < inventory.slots.Count; i++)
        {
            var slot = inventory.slots[i];
            slotUIs[i].Set(slot.item, slot.amount);
        }
    }

    public void OpenCloseInventory()
    {
        isOpened = !isOpened;
        if (isOpened)
        {
            if (slotContainer == null) return;

            if (canvasGroup == null)
                canvasGroup = slotContainer.GetComponent<CanvasGroup>();

            slotContainer.transform.localScale = Vector3.zero;
            slotContainer.SetActive(true);
        
            containerTween?.Kill();
        
            containerTween = slotContainer?.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
            if (canvasGroup != null)
                canvasGroup.DOFade(1f, 0.25f);
        }
        else
        {
            if (slotContainer == null) return;

            containerTween?.Kill();
        
            containerTween = slotContainer?.transform.DOScale(0f, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => slotContainer.SetActive(false));

            if (canvasGroup != null)
                canvasGroup.DOFade(0f, 0.2f);
        }
    }
}
