using Fractura.CraftingSystem;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public class WorldEventManager : SingletonMonoBehaviour<WorldEventManager>
{
    public event Action<CraftingObject> OnDynamicOutcomeExecuted;

    [SerializeField] private List<DynamicOutcome> dynamicOutcomes;

    [Header("Objective Texts")]
    [SerializeField] private TextMeshProUGUI woodFireText;
    [SerializeField] private TextMeshProUGUI chickenText;
    [SerializeField] private TextMeshProUGUI villageObjectiveText;


    [Header("Wood Fire stuff")]
    [SerializeField] private List<Vector3Int> woodFirePositions;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase woodFireSprite;

    [Header("Village stuff")]
    [SerializeField] private bool isChickenMade = false;
    [SerializeField] private bool isWoodfireMade = false;
    [SerializeField] private bool villageObjectiveDone = false;
    [SerializeField] private GameObject boneWeapon;
    [SerializeField] private Vector2 boneWeaponPos;


    public void BroadcastOutcome(CraftingObject executedObject)
    {
        Debug.Log($"WorldEventManager: Outcome executed for {executedObject.ObjectName}");
        OnDynamicOutcomeExecuted?.Invoke(executedObject);

        switch (executedObject.effectType)
        {
            case CraftingEffectType.ChickenMeal:
                Debug.Log("got them chickens");
                //NPCManager.Instance.OnChickenServed();
                CraftingUIManager.Instance.AddLog("<color=green>Chicken</color> Objective Completed.");
                chickenText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;
                isChickenMade = true;
                //chickenText.fontWeight = FontWeight.Bold;
                break;
            case CraftingEffectType.WoodFire:
                Debug.Log("Wood fire made");
                SwapAreaTiles();
                CraftingUIManager.Instance.AddLog("<color=green>Wood Fire</color> Objective Completed.");
                woodFireText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;
                isWoodfireMade = true;
                break;
            default:
                // No dynamic effect.
                break;
        }


        // alternative for designer friendly mechanism
        //foreach (var outcome in dynamicOutcomes)
        //{
        //    if (outcome.effectType == executedObject.effectType)
        //    {
        //        outcome.OnOutcomeExecuted.Invoke();
        //    }
        //}

        if (!villageObjectiveDone)
        {
            if(isChickenMade && isWoodfireMade)
            {
                NPCManager.Instance.OnChickenServed();
                CraftingUIManager.Instance.AddLog("<color=green>You fed the village.</color>");
                villageObjectiveText.fontStyle = FontStyles.Strikethrough | FontStyles.Bold | FontStyles.Italic;
                villageObjectiveDone = true;

                CraftingUIManager.Instance.AddLog("The villagers have gifted you the bone weapon. Pick it up!");
                Instantiate(boneWeapon, boneWeaponPos, Quaternion.identity);
            }
        }

    }

    private void SwapAreaTiles()
    {
        foreach(Vector3Int pos in woodFirePositions)
        {
            tileMap.SetTile(pos, woodFireSprite);
        }
    }
}



[Serializable]
public class DynamicOutcome
{
    [Tooltip("The effect type that triggers this outcome")]
    public CraftingEffectType effectType;

    [Tooltip("The actions to perform when this outcome is executed.")]
    public UnityEvent OnOutcomeExecuted;
}