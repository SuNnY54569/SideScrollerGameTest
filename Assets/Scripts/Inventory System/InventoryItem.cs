using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public enum ItemType
    {
        Resource,
        Tool,
        CraftedObject,
        Seed
    }
    
    public string itemName;
    public Sprite icon;
    public int maxStack = 10;
    public ItemType itemType;
    public bool isStackable = true;
    public GameObject worldDisplayPrefab;
}
