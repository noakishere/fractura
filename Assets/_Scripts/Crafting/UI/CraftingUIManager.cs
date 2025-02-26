using Fractura.CraftingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingUIManager : SingletonPersistentMonoBehaviour<CraftingUIManager>
{
    [SerializeField] private GameObject craftingTableObject;
    public GameObject CraftingTableObject => craftingTableObject;
    [SerializeField] private CraftingTableItemBehaviour[] craftingTableItems = new CraftingTableItemBehaviour[4];

    [SerializeField] private TextMeshProUGUI textMeshProItemInteraction;

    private void OnEnable()
    {
        CraftingManager.Instance.OnCraftingFailed += CraftingFailed;
    }

    private void OnDisable()
    {
        CraftingManager.Instance.OnCraftingFailed -= CraftingFailed;
    }

    public void OpenCraftingTab()
    {
        craftingTableObject.SetActive(!craftingTableObject.active);
    }

    public void CloseCraftingTab()
    {
        craftingTableObject.SetActive(false);
    }

    public void AttemptCraft()
    {
        // do stuff
        CraftingManager.Instance.TryCraft();

        // flush the table
        foreach(CraftingTableItemBehaviour item in craftingTableItems)
        {
            item.EmptyItem();
        }
    }

    public void AddItemToTable(CraftingObject newObject)
    {
        foreach(CraftingTableItemBehaviour item in craftingTableItems)
        {
            if(!item.IsAssigned)
            {
                item.AssignItem(newObject);
                Debug.Log($"{gameObject.name}: Assigning {newObject.name} to {item.gameObject.name}");
                break;
            }
        }
    }

    public void ShowItemTextOnScreen(Vector3 screenPos)
    {
        textMeshProItemInteraction.transform.position = screenPos;
        textMeshProItemInteraction.text = $"(E)";
        textMeshProItemInteraction.gameObject.SetActive(true);
    }

    public void HideItemTextOnScreen()
    {
        if (textMeshProItemInteraction != null)
        {
            textMeshProItemInteraction.gameObject.SetActive(false);
        }
    }

    private void CraftingFailed(CraftingObject obj)
    {
        Debug.Log("CRAFTING FAILED");
    }
}
