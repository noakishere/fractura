using Fractura.CraftingSystem;
using System;
using UnityEditor.EditorTools;
using UnityEngine;

public class WorldEventManager : SingletonMonoBehaviour<WorldEventManager>
{
    public event Action<CraftingObject> OnDynamicOutcomeExecuted;

    public void BroadcastOutcome(CraftingObject executedObject)
    {
        Debug.Log($"WorldEventManager: Outcome executed for {executedObject.ObjectName}");
        OnDynamicOutcomeExecuted?.Invoke(executedObject);

        switch (executedObject.effectType)
        {
            case CraftingEffectType.ChickenMeal:
                Debug.Log("got them chickens");
                //    NPCManager.Instance.UpdateDialogue("chickenMeal");
                break;
            //case CraftingEffectType.Sword:
            //    DoorManager.Instance.CloseDoor("mainEntrance");
            //    break;
            default:
                // No dynamic effect.
                break;
        }
    }
}
