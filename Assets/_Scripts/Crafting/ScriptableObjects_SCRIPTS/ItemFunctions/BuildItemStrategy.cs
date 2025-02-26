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

        BuildOutcomeParameters outcomeParameters = parameters as BuildOutcomeParameters;

        BuildCraftingObject buildCraftingObject = data as BuildCraftingObject;

        var newObj = Object.Instantiate(buildCraftingObject.buildGameObject, buildCraftingObject.buildPosition, Quaternion.identity);

        Inventory.Instance.RemoveItems(data, 1);

        //WeaponOutcomeParameters weaponOutcomeParameters = parameters as WeaponOutcomeParameters;

        //Debug.Log(weaponOutcomeParameters.nameTest);
    }

    public void ExecuteOutcome(CraftingObject data, Vector2 position, OutcomeParameters parameters)
    {
        Debug.Log($"I was called by {position}");

        BuildOutcomeParameters outcomeParameters = parameters as BuildOutcomeParameters;

        var newObj = Object.Instantiate(outcomeParameters.buildingPrefab, position, Quaternion.identity);

        //WeaponOutcomeParameters weaponOutcomeParameters = parameters as WeaponOutcomeParameters;

        //Debug.Log(weaponOutcomeParameters.nameTest);
    }
}
