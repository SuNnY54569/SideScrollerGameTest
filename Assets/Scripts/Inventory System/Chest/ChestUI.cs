using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance { get; private set; }

    [SerializeField] private GameObject chestPanel;
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private Transform slotContainer;

    private CanvasGroup canvasGroup;
    private List<InventorySlotUI> slotUIs = new();
    private Inventory currentChestInventory;
    private Tween panelTween;
    
    public bool IsOpen => chestPanel.activeSelf;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        canvasGroup = chestPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = chestPanel.AddComponent<CanvasGroup>();
        }

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
        chestPanel.transform.localScale = Vector3.zero;
        canvasGroup.alpha = 0;

        panelTween?.Kill();
        panelTween = chestPanel.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, 0.25f);

        PrepareSlots();
        currentChestInventory.OnInventoryChanged.AddListener(UpdateUI);

        InventoryUI.Instance.ForceOpen();
        UpdateUI();
    }

    public void Close()
    {
        if (currentChestInventory != null)
        {
            currentChestInventory.OnInventoryChanged.RemoveListener(UpdateUI);
        }

        InventoryUI.Instance.ForceClose();
        panelTween?.Kill();
        panelTween = chestPanel.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                chestPanel.SetActive(false);
                currentChestInventory = null;
            });
        canvasGroup.DOFade(0f, 0.2f);
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
