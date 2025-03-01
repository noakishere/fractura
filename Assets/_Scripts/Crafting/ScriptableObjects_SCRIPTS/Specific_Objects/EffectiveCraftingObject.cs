using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    [CreateAssetMenu(fileName = "NewEffectiveCraftingObjectData", menuName = "CraftingSystem/Effective Crafting Object Data", order = 1)]
    public class EffectiveCraftingObject : CraftingObject
    {
        public Vector2 buildPosition;
        public GameObject buildGameObject;

        private void OnEnable()
        {
            // Assign the strategy and configure its parameters.
            outcomeStrategy = new EffectiveItemStrategy();
            outcomeParameters = new BuildOutcomeParameters()
            {
                buildPosition = Vector2.zero,
                buildingPrefab = null
            };
        }

        //private void OnValidate()
        //{
        //    outcomeParameters = new EffectiveItemStrategy()
        //    {
        //        buildPosition = this.buildPosition,
        //        buildingPrefab = this.buildGameObject
        //    };
        //}
    }
}

