using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    [CreateAssetMenu(fileName = "NewBuildCraftingObjectData", menuName = "CraftingSystem/Crafting Build Object Data", order = 1)]
    public class BuildCraftingObject : CraftingObject
    {
        public Vector2 buildPosition;
        public GameObject buildGameObject;

        private void OnEnable()
        {
            // Assign the strategy and configure its parameters.
            outcomeStrategy = new BuildItemStrategy();
            outcomeParameters = new BuildOutcomeParameters()
            {
                buildPosition = Vector2.zero,
                buildingPrefab = null
            };
        }

        private void OnValidate()
        {
            outcomeParameters = new BuildOutcomeParameters()
            {
                buildPosition = this.buildPosition,
                buildingPrefab = this.buildGameObject
            };
        }
    }
}
