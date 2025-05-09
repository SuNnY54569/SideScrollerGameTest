using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemUser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuickBar quickBar;
    public GameObject worldItemPickupPrefab;
    [SerializeField] private GameObject wandProjectilePrefab;
    [SerializeField] private float fireCooldown = 0.5f;
    private float lastFireTime = -Mathf.Infinity;
    
    [Header("Drop Animation Settings")]
    [SerializeField] private float minDropDistance = 0.5f;
    [SerializeField] private float maxDropDistance = 1.5f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.5f;
    
    private void Update()
    {
        // Example usage trigger (Space or Left Click)
        if (Input.GetButtonDown("Jump"))
        {
            UseSelectedItem();
        }

        // Drop selected item (Q Key)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            DropSelectedItem();
        }
    }
    
    private void UseSelectedItem()
    {
        var slot = quickBar.GetSelectedSlot();
        if (slot == null || slot.IsEmpty) return;

        if (slot.item.itemName == "Wand") // You can improve this later with item types
        {
            FireProjectile();
        }
    }
    
    public void DropSelectedItem()
    {
        var slot = quickBar.GetSelectedSlot();
        if (slot == null || slot.IsEmpty) return;

        SpawnWorldItem(slot.item, slot.amount, Vector3.one * 0.5f);
        slot.Clear();
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
        
        lastFireTime = Time.time;
        
        Vector3 spawnPosition = transform.position;
        float facingDirection = Mathf.Sign(transform.localScale.x); // +1 or -1
        Vector2 shootDirection = Vector2.right * facingDirection;
        
        var projectile = Instantiate(wandProjectilePrefab, spawnPosition, Quaternion.identity);
        
        var projectileComponent = projectile.GetComponent<WandProjectile>();
        if (projectileComponent != null)
        {
            projectileComponent.SetDirection(shootDirection);
        }
    }
}
