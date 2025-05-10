using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject recipeButtonPrefab;
    [SerializeField] private Transform buttonContainer;
    [SerializeField] private List<CraftingRecipe> allRecipes;
    [SerializeField] private CraftingManager craftingManager;
    [SerializeField] private Inventory inventory;
    private List<CraftingRecipeButton> recipeButtons = new();

    private void Awake()
    {
        if (craftingManager == null)
        {
            craftingManager = FindObjectOfType<CraftingManager>();
        }
        
        if (inventory == null)
            inventory = FindObjectOfType<Inventory>();

        inventory.OnInventoryChanged.AddListener(RefreshAvailability);
    }

    private void OnEnable()
    {
        RefreshMenu();
        if (craftingManager != null && craftingManager.PlayerInventory != null)
        {
            craftingManager.PlayerInventory.OnInventoryChanged.AddListener(RefreshMenu);
        }
    }
    
    private void OnDisable()
    {
        if (craftingManager != null && craftingManager.PlayerInventory != null)
        {
            craftingManager.PlayerInventory.OnInventoryChanged.RemoveListener(RefreshMenu);
        }
    }
    
    private void OnDestroy()
    {
        if (inventory != null)
            inventory.OnInventoryChanged.RemoveListener(RefreshAvailability);
    }

    public void RefreshMenu()
    {
        bool nearWorkbench = craftingManager.IsNearWorkbench();
        
        
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        recipeButtons.Clear();

        foreach (CraftingRecipe recipe in allRecipes)
        {
            if (recipe.craftingType == CraftingType.Workbench && !nearWorkbench)
                continue;

            GameObject buttonObj = Instantiate(recipeButtonPrefab, buttonContainer);
            CraftingRecipeButton button = buttonObj.GetComponent<CraftingRecipeButton>();
            button.Setup(recipe, craftingManager);
            recipeButtons.Add(button);
        }

        RefreshAvailability();
    }
    
    private void RefreshAvailability()
    {
        foreach (var button in recipeButtons)
        {
            bool canCraft = craftingManager.CanCraft(button.Recipe);
            button.UpdateAvailability(canCraft);
        }
    }
}
