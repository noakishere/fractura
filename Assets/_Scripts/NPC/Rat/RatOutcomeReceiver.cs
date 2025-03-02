using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fractura.CraftingSystem;

public class RatOutcomeReceiver : MonoBehaviour, IOutcomeReceiver
{
    public void ReceiveOutcome(CraftingObject craftingObject)
    {
        CraftingUIManager.Instance.AddLog("<color=red>Rat</color> received a damage.");
        Destroy(gameObject);
    }
}
