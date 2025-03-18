using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fractura.CraftingSystem;

public class RatOutcomeReceiver : MonoBehaviour, IOutcomeReceiver
{
    [SerializeField] private GameObject ratMeat;
    public void ReceiveOutcome(CraftingObject craftingObject)
    {
        CraftingUIManager.Instance.AddLog("<color=red>Rat</color> received a damage.");
        WorldEventManager.Instance.IncreaseRatKillCount(1);
        Instantiate(ratMeat, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
