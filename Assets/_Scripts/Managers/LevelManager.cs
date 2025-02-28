using Fractura.CraftingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    [SerializeField] private List<CraftingObjective> objectives;

    private void OnEnable()
    {
        // For each objective in the level, subscribe to its required object's execution event.
        foreach (var objective in objectives)
        {
            if (objective.requiredObject != null)
            {
                objective.requiredObject.OnObjectExecuted += OnCraftingObjectExecuted;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var objective in objectives)
        {
            if (objective.requiredObject != null)
            {
                objective.requiredObject.OnObjectExecuted -= OnCraftingObjectExecuted;
            }
        }
    }

    public bool IsLevelComplete()
    {
        foreach (var obj in objectives)
        {
            if (!obj.IsComplete())
                return false;
        }
        return true;
    }

    private void OnCraftingObjectExecuted(CraftingObject executedObject)
    {
        foreach (var objective in objectives)
        {
            if (objective.requiredObject == executedObject)
            {
                objective.currentCount++;
                Debug.Log($"Objective updated: {executedObject.ObjectName} count is now {objective.currentCount}");
            }
        }

        CheckWinCondition();
    }

    private void OnItemAdded(CraftingObject craftedItem)
    {
        foreach (var objective in objectives)
        {
            if (objective.requiredObject == craftedItem)
            {
                objective.currentCount = Inventory.Instance.GetItemCount(craftedItem);
                Debug.Log($"Objective updated: {craftedItem.ObjectName} count is now {objective.currentCount}");
            }
        }

        // Check for win condition after updating.
        CheckWinCondition();
    }

    private void CheckWinCondition()
    {
        if (IsLevelComplete())
        {
            Debug.Log("Level Complete!");
        }
    }
}


[Serializable]
public class CraftingObjective
{
    [Tooltip("The crafted object that is required.")]
    public CraftingObject requiredObject;

    [Tooltip("The number of this object required.")]
    public int requiredCount;

    [Tooltip("Should the object also be placed in the scene?")]
    public bool requiresPlacement;

    // This field could be updated during gameplay.
    [NonSerialized] public int currentCount;

    /// <summary>
    /// Checks if this objective is complete.
    /// </summary>
    public bool IsComplete()
    {
        return currentCount >= requiredCount;
    }
}