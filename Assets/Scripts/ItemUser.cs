using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ItemUser : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuickBar quickBar;
    public GameObject worldItemPickupPrefab;
    
    [Header("Drop Animation Settings")]
    [SerializeField] private float jumpDistance = 1f;
    [SerializeField] private float jumpHeight = 1f;
    [SerializeField] private float jumpDuration = 0.5f;
    
    private void Update()
    {
        // Example usage trigger (Space or Left Click)
        if (Input.GetMouseButtonDown(0))
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

        Debug.Log($"Using {slot.item.itemName} from quick bar");
        // Expand this for tool use, planting, etc.
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

        var dropObj = Instantiate(worldItemPickupPrefab, dropPosition, Quaternion.identity);
        var pickupComponent = dropObj.GetComponent<WorldItemPickup>();

        if (pickupComponent != null)
        {
            pickupComponent.SetItem(item, amount);
            pickupComponent.transform.localScale = customScale;

            // Calculate target position slightly forward
            Vector3 targetPosition = dropPosition + dropDirection * jumpDistance;

            // Play jump animation and snap to ground after
            dropObj.transform.DOJump(targetPosition, jumpHeight, 1, jumpDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // Snap to ground after jump completes
                    Vector3 groundedPosition = new Vector3(
                        dropObj.transform.position.x,
                        -2f,
                        dropObj.transform.position.z
                    );
                    dropObj.transform.position = groundedPosition;
                });
        }
    }
}
