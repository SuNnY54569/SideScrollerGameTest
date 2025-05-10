using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipeButton : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    private CraftingRecipe recipe;
    private CraftingManager craftingManager;
    public CraftingRecipe Recipe => recipe;

    public void Setup(CraftingRecipe newRecipe, CraftingManager manager)
    {
        recipe = newRecipe;
        craftingManager = manager;
        label.text = recipe.result.itemName;
        icon.sprite = recipe.result.icon;
    }

    public void OnClick()
    {
        craftingManager.Craft(recipe);
    }
    
    public void UpdateAvailability(bool canCraft)
    {
        button.image.color = canCraft ? Color.white : Color.gray;
    }
}
