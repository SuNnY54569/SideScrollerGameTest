using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text countText;
    [SerializeField] private GameObject dragIconPrefab;
    
    [SerializeField] private Image backgroundImage;
    [ColorUsage(true, true)] [SerializeField] private Color normalColor = Color.white;
    [ColorUsage(true, true)] [SerializeField] private Color selectedColor = new Color(0.7f, 0.7f, 0.7f);
    
    public int slotIndex;
    private Inventory inventory;
    private QuickBar quickBar;
    private bool isQuickBarSlot;
    
    private GameObject dragIconInstance;
    private static InventoryItem draggedItem;
    private static int draggedAmount;
    
    private static Inventory sourceInventory;
    private static QuickBar sourceQuickBar;
    private static int sourceSlotIndex;
    
    public void Initialize(Inventory inv, QuickBar quick, int index, bool isQuickBar = false)
    {
        inventory = inv;
        quickBar = quick;
        slotIndex = index;
        this.isQuickBarSlot = isQuickBar;
    }
    
    public void Set(InventoryItem item, int amount)
    {
        if (item != null)
        {
            iconImage.sprite = item.icon;
            iconImage.enabled = true;
            countText.text = item.maxStack > 1 ? amount.ToString() : "";
        }
        else
        {
            Clear();
        }
    }
    
    public void SetSelected(bool isSelected)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = isSelected ? selectedColor : normalColor;
        }
    }

    public void Clear()
    {
        iconImage.sprite = null;
        iconImage.enabled = false;
        countText.text = "";
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        InventorySlot slot = isQuickBarSlot ? quickBar.quickSlots[slotIndex] : inventory.slots[slotIndex];
        if (slot.IsEmpty) return;

        draggedItem = slot.item;
        draggedAmount = slot.amount;
        sourceSlotIndex = slotIndex;
        sourceInventory = inventory;
        sourceQuickBar = quickBar;
        
        dragIconInstance = Instantiate(dragIconPrefab, transform.root);
        Image icon = dragIconInstance.GetComponentInChildren<Image>();
        TMP_Text count = dragIconInstance.GetComponentInChildren<TMP_Text>();

        icon.sprite = draggedItem.icon;
        count.text = draggedItem.maxStack > 1 ? draggedAmount.ToString() : "";
        dragIconInstance.transform.position = Input.mousePosition;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (dragIconInstance != null)
        {
            dragIconInstance.transform.position = Input.mousePosition;
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (dragIconInstance != null)
        {
            Destroy(dragIconInstance);
            dragIconInstance = null;
        }

        draggedItem = null;
        draggedAmount = 0;
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        if (draggedItem == null) return;
        
        InventorySlot destinationSlot = isQuickBarSlot
            ? quickBar.quickSlots[slotIndex]
            : inventory.slots[slotIndex];
        
        InventoryItem oldItem = destinationSlot.item;
        int oldAmount = destinationSlot.amount;
        
        destinationSlot.item = draggedItem;
        destinationSlot.amount = draggedAmount;
        
        if (sourceInventory != null)
        {
            sourceInventory.slots[sourceSlotIndex].item = oldItem;
            sourceInventory.slots[sourceSlotIndex].amount = oldAmount;
        }
        else if (sourceQuickBar != null)
        {
            sourceQuickBar.quickSlots[sourceSlotIndex].item = oldItem;
            sourceQuickBar.quickSlots[sourceSlotIndex].amount = oldAmount;
        }
        
        draggedItem = null;
        draggedAmount = 0;
        sourceInventory = null;
        sourceQuickBar = null;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        if (isQuickBarSlot && quickBar != null)
        {
            quickBar.SetSelectedIndex(slotIndex);
        }
        
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            DropSlotItem();
        }
        
    }
    
    private void DropSlotItem()
    {
        var slot = isQuickBarSlot ? quickBar.quickSlots[slotIndex] : inventory.slots[slotIndex];
        if (slot.IsEmpty) return;

        var player = FindObjectOfType<ItemUser>();
        if (player != null)
        {
            player.SpawnWorldItem(slot.item, slot.amount, Vector3.one * 0.5f);
        }

        slot.Clear();
    }

}
