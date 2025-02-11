using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image img;
    public Image CraftingTableImage => img;

    [SerializeField] private bool isAssigned;
    public bool IsAssigned => isAssigned;

    [SerializeField] private bool isOnTheTable;
    public bool IsOnTheTable => isOnTheTable;

    [SerializeField] private CraftingObject objectReference;
    public CraftingObject ObjectReference => objectReference;

    public void AssignItem(CraftingObject newObj)
    {
        if (!isAssigned)
        {
            objectReference = newObj;
            img.sprite = newObj.ObjectSprite;
            Debug.Log($"<b>{gameObject.name}</b>: Image renderer's image is set to {newObj.ObjectSprite}");
            
            isAssigned = true;
            isOnTheTable = false;
            
            ToggleImageRenderer(true);
        }
        else
        {
            Debug.Log($"{gameObject.name}: Image renderer is already assigned");
        }
    }

    public void ToggleImageRenderer(bool state)
    {
        img.color = Color.white;
        img.enabled = state;
        Debug.Log($"{gameObject.name}: Image renderer is set to {state}");
    }

    public void RemoveItem(CraftingObject objectToBeRemoved)
    {
        if(objectReference != null & objectReference == objectToBeRemoved)
        {
            EmptyItem();            
        }
    }

    public void EmptyItem()
    {
        isAssigned = false;
        img.sprite = null;
        ToggleImageRenderer(false);
        Debug.Log($"{gameObject.name}: Emptied and nulled.");
    }

    public void SendToTable()
    {
        if(isAssigned && !isOnTheTable)
        {
            CraftingManager.Instance.AddItemToTable(objectReference);
            isOnTheTable = true;
            img.color = Color.green;
        }
    }

    public void ReturnFromTable()
    {
        isOnTheTable = false;
        img.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(!isOnTheTable)
        {
            img.color = Color.white;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isOnTheTable)
        {
            img.color = Color.red;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("RIGHT CLICK");
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SendToTable();
        }
    }
}
