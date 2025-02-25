using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingObject", menuName = "CraftingSystem/CraftingObjectScriptableObject", order = 1)]
public class SOCraftingObjects : ScriptableObject
{
    [SerializeField] private List<CraftingObject> craftingObjects;
    public List<CraftingObject> CraftingObjects => craftingObjects;
}
