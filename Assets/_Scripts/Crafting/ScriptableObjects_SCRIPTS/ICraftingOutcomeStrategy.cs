
using UnityEngine;

namespace Fractura.CraftingSystem
{
    public interface ICraftingOutcomeStrategy
    {
        void ExecuteOutcome(CraftingObject data, GameObject user, OutcomeParameters parameters);
    }
}

