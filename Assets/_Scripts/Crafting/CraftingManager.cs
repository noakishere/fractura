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
            // Build a frequency dictionary for the placed ingredients.
            Dictionary<string, int> placedCounts = new Dictionary<string, int>();
            foreach (var obj in tableCraftingObjects)
            {
                string key = obj.ObjectName;
                if (placedCounts.ContainsKey(key))
                    placedCounts[key]++;
                else
                    placedCounts[key] = 1;
            }

            foreach (var recipe in craftingObjectsDB.CraftingObjects)
            {
                Dictionary<string, int> recipeCounts = new Dictionary<string, int>();
                foreach (var ingredient in recipe.Recipe.Ingredients)
                {
                    string key = ingredient.Ingredient.ObjectName;

                    // Assume each ingredient has a Quantity property.
                    if (recipeCounts.ContainsKey(key))
                        recipeCounts[key] += ingredient.Quantity;
                    else
                        recipeCounts[key] = ingredient.Quantity;
                }

                if (placedCounts.Count != recipeCounts.Count)
                    continue;

                bool isMatch = true;
                foreach (var kvp in recipeCounts)
                {
                    if (!placedCounts.TryGetValue(kvp.Key, out int placedCount) || placedCount != kvp.Value)
                    {
                        isMatch = false;
                        break;
                    }
                }

                if (isMatch)
                    return recipe;
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
            bool attemptAdd = playerInventory.AttemptAddItem(itemData);
            
            if(attemptAdd)
            {
                // Remove ingredients from inventory.
                foreach (var ingredient in itemData.Recipe.Ingredients)
                {
                    playerInventory.RemoveItems(ingredient.Ingredient, ingredient.Quantity);
                }

                OnCraftingCompleted?.Invoke(itemData);
                Debug.Log("Crafting completed: " + itemData.ObjectName);
            }
            else
            {
                OnCraftingFailed?.Invoke(itemData);
                Debug.LogWarning("Crafting Failed Because Stack is full for: " + itemData.ObjectName);
            }
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
    }
}
