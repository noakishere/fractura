using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildItemStrategy : ICraftingOutcomeStrategy
{
    public void ExecuteOutcome(CraftingObject data, GameObject user, OutcomeParameters parameters)
    {
        Debug.Log($"I was called by {user}");

        BuildCraftingObject buildCraftingObject = data as BuildCraftingObject;

        var newObj = Object.Instantiate(buildCraftingObject.buildGameObject, buildCraftingObject.buildPosition, Quaternion.identity);

        Inventory.Instance.RemoveItems(data, 1);
    }
}
