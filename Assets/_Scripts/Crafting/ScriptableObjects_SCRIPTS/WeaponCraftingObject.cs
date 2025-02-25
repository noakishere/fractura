using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    [CreateAssetMenu(fileName = "NewWeaponCraftingObjectData", menuName = "CraftingSystem/Crafting Weapon Object Data", order = 1)]
    public class WeaponCraftingObject : CraftingObject
    {
        private void OnEnable()
        {
            // Assign the strategy and configure its parameters.
            outcomeStrategy = new WeaponItemStrategy();
            outcomeParameters = new WeaponOutcomeParameters
            {
                nameTest = "hello im a weapon"
            };
        }
    }
}
