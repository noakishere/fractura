using Fractura.CraftingSystem;
using System;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldEventManager : SingletonMonoBehaviour<WorldEventManager>
{
    public event Action<CraftingObject> OnDynamicOutcomeExecuted;

    [Header("Wood Fire stuff")]
    [SerializeField] private List<Vector3Int> woodFirePositions;
    [SerializeField] private Tilemap tileMap;
    [SerializeField] private TileBase woodFireSprite;

    public void BroadcastOutcome(CraftingObject executedObject)
    {
        Debug.Log($"WorldEventManager: Outcome executed for {executedObject.ObjectName}");
        OnDynamicOutcomeExecuted?.Invoke(executedObject);

        switch (executedObject.effectType)
        {
            case CraftingEffectType.ChickenMeal:
                Debug.Log("got them chickens");
                NPCManager.Instance.OnChickenServed();
                CraftingUIManager.Instance.AddLog("Chicken Objective Completed.");
                break;
            case CraftingEffectType.WoodFire:
                Debug.Log("Wood fire made");
                SwapAreaTiles();
                CraftingUIManager.Instance.AddLog("Wood Fire Objective Completed.");
                break;
            default:
                // No dynamic effect.
                break;
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
