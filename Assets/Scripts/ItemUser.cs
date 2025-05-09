using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemUser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuickBar quickBar;
    [SerializeField] private ArcanePower arcanePower;
    [SerializeField] private GameObject wandProjectilePrefab;
    [SerializeField] private float fireCooldown = 0.5f;
    [SerializeField] private Inventory inventory;
    public GameObject worldItemPickupPrefab;
    private float lastFireTime = -Mathf.Infinity;
    
    [Header("Drop Animation Settings")]
    [SerializeField] private float minDropDistance = 0.5f;
    [SerializeField] private float maxDropDistance = 1.5f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.5f;

    private void Awake()
    {
        if (inventory == null)
        {
            inventory.GetComponent<Inventory>();
        }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            UseSelectedItem();
        }
        
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropSelectedItem();
        }
    }
    
    private void UseSelectedItem()
    {
        var slot = quickBar.GetSelectedSlot();
        if (slot == null || slot.IsEmpty) return;
        var item = slot.item;
        
        if (item.placeablePrefab != null)
        {
            PlaceObject(item, slot);
            return;
        }

        if (item.itemName == "Wand") // You can improve this later with item types
        {
            FireProjectile();
        }
    }
    
    private void PlaceObject(InventoryItem item, InventorySlot slot)
    {
        Vector3 placePosition = transform.position;
        Instantiate(item.placeablePrefab, placePosition, Quaternion.identity);

        slot.RemoveItem(1);
    }
    
    public void DropSelectedItem()
    {
        var slot = quickBar.GetSelectedSlot();
        if (slot == null || slot.IsEmpty) return;

        SpawnWorldItem(slot.item, slot.amount, Vector3.one * 0.5f);
        slot.Clear();
        inventory.OnInventoryChanged.Invoke();
    }
    
    public void SpawnWorldItem(InventoryItem item, int amount,  Vector3 customScale)
    {
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector3 dropDirection = Vector3.right * facingDirection;
        Vector3 dropPosition = transform.position + dropDirection * 1f;
        
        float randomDistance = Random.Range(minDropDistance, maxDropDistance);
        Vector3 targetPosition = dropPosition + dropDirection * randomDistance;

        RaycastHit2D hit = Physics2D.Raycast(targetPosition, Vector2.down, 10f, LayerMask.GetMask("Ground"));
        
        Vector3 groundTargetPosition = targetPosition;
        if (hit.collider != null)
        {
            groundTargetPosition.y = hit.point.y;
        }
        else
        {
            Debug.LogWarning("No ground detected below target position. Using current target Y.");
        }
        
        var dropObj = Instantiate(worldItemPickupPrefab, dropPosition, Quaternion.identity);
        var pickupComponent = dropObj.GetComponent<WorldItemPickup>();

        if (pickupComponent != null)
        {
            pickupComponent.SetItem(item, amount);
            pickupComponent.transform.localScale = customScale;
            
            dropObj.transform.DOJump(groundTargetPosition, jumpHeight, 1, jumpDuration)
                .SetEase(Ease.OutQuad);
        }
    }
    
    private void FireProjectile()
    {
        if (Time.time - lastFireTime < fireCooldown)
            return; 
        
        if (arcanePower != null && !arcanePower.CanUse())
        {
            Debug.Log("Not enough Arcane Power!");
            return;
        }
        
        lastFireTime = Time.time;
        arcanePower.UseAP();
        
        Vector3 spawnPosition = transform.position;
        float facingDirection = Mathf.Sign(transform.localScale.x);
        Vector2 shootDirection = Vector2.right * facingDirection;
        
        GameObject projectileObj = ObjectPoolManager.Instance.Get("Projectile");
        if (projectileObj != null)
        {
            projectileObj.transform.position = spawnPosition;
            projectileObj.transform.rotation = Quaternion.identity;

            WandProjectile projectile = projectileObj.GetComponent<WandProjectile>();
            if (projectile != null)
            {
                projectile.SetDirection(shootDirection);
            }
        }
    }
}
