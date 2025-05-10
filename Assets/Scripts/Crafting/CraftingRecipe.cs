using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum CraftingType
{
    Anywhere,
    Workbench
}

[CreateAssetMenu(menuName = "Crafting/Recipe")]
public class CraftingRecipe : ScriptableObject
{
    public CraftingType craftingType = CraftingType.Anywhere;
    public InventoryItem[] ingredients;
    public int[] ingredientAmounts;
    public InventoryItem result;
    public int resultAmount = 1;
}
