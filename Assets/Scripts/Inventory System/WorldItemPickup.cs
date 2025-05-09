using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class WorldItemPickup : MonoBehaviour
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private int quantity = 1;
    [SerializeField] private GameObject promptUI;
    [SerializeField] private float pickupAnimationDuration = 0.3f;
    [SerializeField] private Transform visual;
    [SerializeField] private Color highlightColor = Color.yellow;
    [SerializeField]private SpriteRenderer spriteRenderer;
    
    private bool playerInRange = false;
    private bool isCollected;
    
    private Inventory playerInventory;
    private Color originalColor = Color.white;
    private Transform playerTransform;
    
    private CanvasGroup canvasGroup;
    private Tween promptTween;
    
    private void OnValidate()
    {
        EnsureCollider();
        UpdateVisuals();
    }
    
    
    private void Awake()
    {
        EnsureCollider();
    }

    private void Start()
    {
        UpdateVisuals();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryPickup();
        }
    }
    
    private void TryPickup()
    {
        if (isCollected) return;
        
        if (playerInventory != null)
        {
            bool success = playerInventory.AddItem(item, quantity);
            if (success)
            {
                isCollected = true;
                Highlight(false);
                if (promptUI != null) promptUI.SetActive(false);
                AnimatePickupAndDestroy();
            }
            else
            {
                Debug.Log("Inventory full!");
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCollected) return;
        
        if (other.CompareTag("Player"))
        {
            playerInventory = other.GetComponent<Inventory>();
            playerTransform = other.transform;
            playerInRange = true;
            Highlight(true);

            ShowPrompt();
        }
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (isCollected) return;
        
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerInventory = null;
            playerTransform = null;
            Highlight(false);

            HidePrompt();
        }
    }
    
    private void Highlight(bool enable)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = enable ? highlightColor : originalColor;
        }
    }
    
    private void AnimatePickupAndDestroy()
    {
        if (visual == null) visual = transform;
        
        promptTween?.Kill();

        visual.DOMove(playerTransform.position, pickupAnimationDuration)
            .SetEase(Ease.InBack)
            .OnComplete(() => Destroy(gameObject));

        visual.DOScale(0, pickupAnimationDuration).SetEase(Ease.InCubic);
    }
    
    private void ShowPrompt()
    {
        if (promptUI == null) return;

        if (canvasGroup == null)
            canvasGroup = promptUI.GetComponent<CanvasGroup>();

        promptUI.transform.localScale = Vector3.zero;
        promptUI.SetActive(true);
        
        promptTween?.Kill();
        
        promptTween = promptUI?.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack);
        if (canvasGroup != null)
            canvasGroup.DOFade(1f, 0.25f);
    }
    
    private void HidePrompt()
    {
        if (promptUI == null) return;

        promptTween?.Kill();
        
        promptTween = promptUI?.transform.DOScale(0f, 0.2f)
            .SetEase(Ease.InBack)
            .OnComplete(() => promptUI.SetActive(false));

        if (canvasGroup != null)
            canvasGroup.DOFade(0f, 0.2f);
    }
    
    public void SetItem(InventoryItem newItem, int newQuantity)
    {
        item = newItem;
        quantity = newQuantity;
    }
    
    private void EnsureCollider()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider == null)
        {
            collider = gameObject.AddComponent<BoxCollider2D>();
        }
        
        collider.isTrigger = true;
        
        if (spriteRenderer != null && spriteRenderer.sprite != null)
        {
            collider.size = spriteRenderer.sprite.bounds.size;
            collider.offset = spriteRenderer.sprite.bounds.center;
        }
    }
    
    private void UpdateVisuals()
    {
        if (item != null && spriteRenderer != null)
        {
            spriteRenderer.sprite = item.icon;
            spriteRenderer.color = Color.white;
            name = $"Pickup_{item.itemName}"; 
        }
    }
}
