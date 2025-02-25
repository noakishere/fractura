using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    public class WeaponItemStrategy : ICraftingOutcomeStrategy
    {
        public void ExecuteOutcome(CraftingObject data, GameObject user, OutcomeParameters parameters)
        {
            WeaponOutcomeParameters weaponOutcomeParameters = parameters as WeaponOutcomeParameters;

            Debug.Log(weaponOutcomeParameters.nameTest);
        }
    }
}
