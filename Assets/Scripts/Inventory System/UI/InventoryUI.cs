using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance { get; private set; }
    
    [SerializeField] private Inventory inventory;
    [SerializeField] private QuickBar quickBar;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject slotContainer;
    [SerializeField] private GameObject backgroundPanel;
    [SerializeField] private GameObject Trashcan;
    [SerializeField] private GameObject craftingMenuPanel;
    
    private List<InventorySlotUI> slotUIs = new();
    private bool isOpened;
    private bool isForceOpened;
    private CanvasGroup craftingCanvasGroup;
    private CanvasGroup canvasGroup;
    private CanvasGroup bgCanvasGroup;
    private Tween containerTween;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        
        if (inventory == null)
        {
            inventory = FindObjectOfType<Inventory>();
        }

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
        UpdateUI();
        
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (ChestUI.Instance != null && ChestUI.Instance.IsOpen)
            {
                // Prevent closing while chest is open
                return;
            }

            
            ToggleInventory();
        }
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
    
    public void ToggleInventory()
    {
        if (ChestUI.Instance.IsOpen) return;
        
        if (isOpened)
            CloseInventory();
        else
            OpenInventory();
        inventory.NotifyChange();
    }
    
    public void OpenInventory()
    {
        if (isOpened) return;
        isOpened = true;

        PrepareUI();
        AnimateOpen();
    }
    
    public void CloseInventory()
    {
        if (!isOpened) return;
        isOpened = false;

        AnimateClose();
    }
    
    private void PrepareUI()
    {
        if (slotContainer == null) return;

        if (canvasGroup == null)
            canvasGroup = slotContainer.GetComponent<CanvasGroup>();
        if (bgCanvasGroup == null && backgroundPanel != null)
            bgCanvasGroup = backgroundPanel.GetComponent<CanvasGroup>();
        if (!isForceOpened)
        {
            if (craftingCanvasGroup == null && craftingMenuPanel != null)
                craftingCanvasGroup = craftingMenuPanel.GetComponent<CanvasGroup>();
        }

        slotContainer.transform.localScale = Vector3.zero;
        slotContainer.SetActive(true);
        backgroundPanel?.SetActive(true);
        
        if (craftingMenuPanel == null || isForceOpened) return;
        craftingMenuPanel.transform.localScale = Vector3.zero;
        craftingMenuPanel.SetActive(true);

    }
    
    private void AnimateOpen()
    {
        containerTween?.Kill();
        containerTween = slotContainer.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack)
            .OnComplete(() => PauseGame(true));;

        canvasGroup?.DOFade(1f, 0.25f);
        bgCanvasGroup?.DOFade(1f, 0.25f);

        if (isForceOpened) return;
        craftingCanvasGroup?.DOFade(1f, 0.25f);
        craftingMenuPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
    }
    
    private void AnimateClose()
    {
        PauseGame(false);
        containerTween?.Kill();
        containerTween = slotContainer.transform.DOScale(0f, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                slotContainer.SetActive(false);
                backgroundPanel?.SetActive(false);
            });

        canvasGroup?.DOFade(0f, 0.2f);
        bgCanvasGroup?.DOFade(0f, 0.2f);
        
        if (isForceOpened) return;
        if (craftingMenuPanel != null)
        {
            craftingMenuPanel.transform.DOScale(0f, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() => craftingMenuPanel.SetActive(false));
            craftingCanvasGroup?.DOFade(0f, 0.2f);
        }
    }
    
    private void PauseGame(bool pause)
    {
        Time.timeScale = pause ? 0f : 1f;
        Debug.Log("pause");
    }
    
    public void ForceOpen()
    {
        if (!isOpened)
        {
            isForceOpened = true;
            OpenInventory();
        }
        
    }
    
    public void ForceClose()
    {
        if (isOpened)
        {
            CloseInventory();
            isForceOpened = false;
        }
    }
}
