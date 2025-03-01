using Fractura.CraftingSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUIManager : SingletonPersistentMonoBehaviour<CraftingUIManager>
{
    [SerializeField] private GameObject craftingTableObject;
    public GameObject CraftingTableObject => craftingTableObject;
    [SerializeField] private CraftingTableItemBehaviour[] craftingTableItems = new CraftingTableItemBehaviour[4];

    [SerializeField] private TextMeshProUGUI textMeshProItemInteraction;
    [SerializeField] private TextMeshProUGUI textMeshProNPC;

    [Header("Logging")]
    [SerializeField] private GameObject scrollBarContentParent;
    [SerializeField] private TextMeshProUGUI logUpdatePrefab;
    [SerializeField] private ScrollRect logScrollRect;

    private void OnEnable()
    {
        CraftingManager.Instance.OnCraftingFailed += CraftingFailed;
    }

    private void OnDisable()
    {
        CraftingManager.Instance.OnCraftingFailed -= CraftingFailed;
    }

    public void ToggleCraftingTab()
    {
        craftingTableObject.SetActive(!craftingTableObject.active);
    }

    public void OpenCraftingTab()
    {
        craftingTableObject.SetActive(true);
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
        CloseCraftingTab();
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
        //textMeshProItemInteraction.text = $"(e)";
        textMeshProItemInteraction.gameObject.SetActive(true);
    }

    public void HideItemTextOnScreen()
    {
        if (textMeshProItemInteraction != null)
        {
            textMeshProItemInteraction.gameObject.SetActive(false);
        }
    }

    public void ShowNPCTextOnScreen(Vector3 screenPos)
    {
        textMeshProNPC.transform.position = screenPos;
        textMeshProNPC.text = $"(e)";
        textMeshProNPC.gameObject.SetActive(true);
    }

    public void HideNPCTextOnScreen()
    {
        if (textMeshProNPC != null)
        {
            textMeshProNPC.gameObject.SetActive(false);
        }
    }

    private void CraftingFailed(CraftingObject obj)
    {
        Debug.Log("CRAFTING FAILED");
    }

    internal void WriteNPCText(string dialogue)
    {
        textMeshProNPC.text = dialogue;
    }

    public void AddLog(string text)
    {
        TextMeshProUGUI newLog = Instantiate(logUpdatePrefab, scrollBarContentParent.transform);
        newLog.text = text;

        Canvas.ForceUpdateCanvases();

        // Set the scroll rect to the bottom.
        logScrollRect.verticalNormalizedPosition = 0f;
    }
}
