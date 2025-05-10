using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;
    [SerializeField] private CraftingFeedbackUI feedbackUI;
    [SerializeField] private CraftingMenuManager craftingMenuManager;
    [SerializeField] private bool isNearWorkbench = false;

    public Inventory PlayerInventory => playerInventory;
    
    private void Awake()
    {
        if (playerInventory == null)
        {
            playerInventory = GetComponent<Inventory>();
        }
            
        if (feedbackUI == null)
        {
            feedbackUI = FindObjectOfType<CraftingFeedbackUI>();
        }

        if (craftingMenuManager == null)
        {
            craftingMenuManager = FindObjectOfType<CraftingMenuManager>();
        }
    }
    
    public bool CanCraft(CraftingRecipe recipe)
    {
        if (recipe.craftingType == CraftingType.Workbench && !isNearWorkbench)
            return false;

        for (int i = 0; i < recipe.ingredients.Length; i++)
        {
            if (playerInventory.GetItemCount(recipe.ingredients[i]) < recipe.ingredientAmounts[i])
                return false;
        }

        return true;
    }

    public void Craft(CraftingRecipe recipe)
    {
        if (CanCraft(recipe))
        {
            for (int i = 0; i < recipe.ingredients.Length; i++)
            {
                playerInventory.RemoveItem(recipe.ingredients[i], recipe.ingredientAmounts[i]);
            }

            playerInventory.TryMergeOrAddItem(recipe.result, recipe.resultAmount);
            feedbackUI.ShowFeedback($"Crafted {recipe.result.itemName}!");
        }
        else
        {
            feedbackUI.ShowFeedback("Missing Ingredients!");
        }
    }
    
    public void SetNearWorkbench(bool isNear)
    {
        isNearWorkbench = isNear;
        craftingMenuManager.RefreshMenu();
    }
    
    public bool IsNearWorkbench()
    {
        return isNearWorkbench;
    }
}
