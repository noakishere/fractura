using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    public class CraftingManager : SingletonMonoBehaviour<CraftingManager>
    {
        // Reference to the player's inventory (assign via the Inspector or find at runtime)
        [SerializeField] private Inventory playerInventory;
        [SerializeField] private List<CraftingObject> tableCraftingObjects;

        [SerializeField] private SOCraftingObjects craftingObjectsDB;

        #region Actions
        public event Action<CraftingObject> OnCraftingStarted;
        public event Action<CraftingObject> OnCraftingCompleted;
        public event Action<CraftingObject> OnCraftingFailed;
        #endregion

        public void TryCraft()
        {
            CraftingObject objectToCraft = FindMatchingRecipe();

            if (objectToCraft != null)
            {
                OnCraftingStarted?.Invoke(objectToCraft);
                Debug.Log($"{gameObject.name}: Crafting started: " + objectToCraft.ObjectName);

                // Optionally, simulate crafting time.
                if (objectToCraft.CraftingTime > 0f)
                {
                    StartCoroutine(CraftWithDelay(objectToCraft));
                }
                else
                {
                    CompleteCrafting(objectToCraft);
                }
                Debug.Log($"{gameObject.name}: Crafted: {objectToCraft.ObjectName}");
            }
            else
            {
                OnCraftingFailed?.Invoke(objectToCraft);
                Debug.Log($"{gameObject.name}: no matches found");
            }
        }

        private CraftingObject FindMatchingRecipe()
        {
            List<string> placedIDs = tableCraftingObjects.Select(ing => ing.ObjectName).OrderBy(id => id).ToList();

            foreach(string str in placedIDs)
            {
                Debug.Log(str);
            }

            foreach (var recipe in craftingObjectsDB.CraftingObjects)
            {
                List<string> recipeIDs = recipe.Recipe.Ingredients.Select(ing => ing.Ingredient.ObjectName).OrderBy(id => id).ToList();

                // Check if the counts match and if every ingredient is the same.
                if (placedIDs.Count == recipeIDs.Count && placedIDs.SequenceEqual(recipeIDs))
                {
                    return recipe;
                }
            }
            return null;
        }

        private IEnumerator CraftWithDelay(CraftingObject itemData)
        {
            yield return new WaitForSeconds(itemData.CraftingTime);
            CompleteCrafting(itemData);
        }

        private void CompleteCrafting(CraftingObject itemData)
        {
            // Remove ingredients from inventory.
            foreach (var ingredient in itemData.Recipe.Ingredients)
            {
                playerInventory.RemoveItems(ingredient.Ingredient, ingredient.Quantity);
            }

            OnCraftingCompleted?.Invoke(itemData);

            // Add the crafted item to the inventory.
            playerInventory.AddItem(itemData);

            // Notify subscribers that crafting completed successfully.
            Debug.Log("Crafting completed: " + itemData.ObjectName);
        }

        public void AddItemToTable(CraftingObject newObj)
        {
            tableCraftingObjects.Add(newObj);
            CraftingUIManager.Instance.AddItemToTable(newObj);
        }

        public void RemoveItemFromTable(CraftingObject newObj)
        {
            if (tableCraftingObjects.Contains(newObj))
            {
                tableCraftingObjects.Remove(newObj);
            }
        }


        #region Archive
        //public bool CanCraft(CraftingObject itemData)
        //{
        //    if (itemData == null || itemData.Recipe == null)
        //    {
        //        Debug.LogWarning("Invalid crafting item or recipe.");
        //        return false;
        //    }

        //    foreach (var ingredient in itemData.Recipe.Ingredients)
        //    {
        //        if (playerInventory.GetItemCount(ingredient.Ingredient) < ingredient.Quantity)
        //        {
        //            return false;
        //        }
        //    }

        //    return true;
        //}

        //public void CraftItem(CraftingObject itemData)
        //{
        //    if (!CanCraft(itemData))
        //    {
        //        Debug.LogWarning("Cannot craft: " + itemData.ObjectName);
        //        OnCraftingFailed?.Invoke(itemData);
        //        return;
        //    }

        //    // Notify subscribers that crafting is starting.
        //    OnCraftingStarted?.Invoke(itemData);
        //    Debug.Log("Crafting started: " + itemData.ObjectName);

        //    // Optionally, simulate crafting time.
        //    if (itemData.CraftingTime > 0f)
        //    {
        //        StartCoroutine(CraftWithDelay(itemData));
        //    }
        //    else
        //    {
        //        CompleteCrafting(itemData);
        //    }
        //}

        #endregion
    }
}
