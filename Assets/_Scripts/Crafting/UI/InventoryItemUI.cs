using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image img;
    public Image CraftingTableImage => img;

    [SerializeField] private bool isAssigned;
    public bool IsAssigned => isAssigned;

    [SerializeField] private bool isOnTheTable;
    public bool IsOnTheTable => isOnTheTable;

    [SerializeField] private int itemsCount;
    [SerializeField] private int tableCount;

    [SerializeField] private CraftingObject objectReference;
    public CraftingObject ObjectReference => objectReference;

    [SerializeField] private KeyCode keyboardNum;

    private CraftingUIManager craftingUIManager;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemCount;


    private void Start()
    {
        craftingUIManager = CraftingUIManager.Instance;
    }

    private void Update()
    {
        if(Input.GetKeyDown(keyboardNum))
        {
            if(objectReference != null)
            {
                if(CraftingUIManager.Instance.CraftingTableObject.activeSelf)
                {
                    SendToTable();
                }
                else
                {
                    objectReference.ExecuteOutcome(Inventory.Instance.gameObject);
                }
            }
        }
    }

    public void AssignItem(CraftingObject newObj, int count)
    {
        if (!isAssigned)
        {
            objectReference = newObj;
            itemsCount = count;
            tableCount = 0;

            img.sprite = newObj.ObjectSprite;

            itemName.text = newObj.ObjectName;
            itemCount.text = count.ToString();


            Debug.Log($"<b>{gameObject.name}</b>: Image renderer's image is set to {newObj.ObjectSprite}");
            
            isAssigned = true;
            isOnTheTable = false;
            
            ToggleImageRenderer(true);
        }
        else if(isAssigned && objectReference == newObj)
        {
            itemCount.text = count.ToString();
            itemsCount = count;
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
        itemCount.transform.parent.gameObject.SetActive(state); //oof
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
        itemsCount = 0;
        isAssigned = false;
        img.sprite = null;
        ToggleImageRenderer(false);
        itemName.text = "";
        objectReference = null;
        Debug.Log($"{gameObject.name}: Emptied and nulled.");
    }

    public void SendToTable()
    {
        if(isAssigned 
            && !isOnTheTable 
            && craftingUIManager.CraftingTableObject.gameObject.activeSelf)
        {
            CraftingManager.Instance.AddItemToTable(objectReference);

            tableCount++;
            if (tableCount >= itemsCount)
            {
                isOnTheTable = true;
            }
            img.color = Color.green;
        }
    }

    public void ReturnFromTable()
    {
        isOnTheTable = false;
        tableCount--;
        img.color = Color.white;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //if(!isOnTheTable)
        //{
            img.color = Color.white;
            itemName.gameObject.SetActive(false);
        //}
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if(!isOnTheTable)
        //{
            img.color = Color.red;
            itemName.gameObject.SetActive(true);
        //}
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (!CraftingUIManager.Instance.CraftingTableObject.activeSelf && objectReference != null)
            {
                if (itemsCount <= 0)
                {
                    EmptyItem();
                    return;
                }

                Vector3 playerPos = Inventory.Instance.gameObject.transform.position;
                Vector3 spawnPos = playerPos + new Vector3(0, 2f, 0);

                // Make sure the item has an associated drop prefab.
                if (PlayerInventoryUIManager.Instance.DropPrefab != null)
                {
                    var newDrop = Instantiate(PlayerInventoryUIManager.Instance.DropPrefab, spawnPos, Quaternion.identity);
                    newDrop.GetComponent<InteractiveTile>().SetObject(objectReference);

                    newDrop.GetComponent<SpriteRenderer>().sprite = objectReference.GetSprite;

                    Debug.Log($"{gameObject.name}: Discarded one {objectReference.ObjectName} at {spawnPos}");
                }
                else
                {
                    Debug.LogWarning($"{objectReference.ObjectName} does not have a dropPrefab assigned!");
                }

                // Update the inventory count and UI:
                itemsCount--;
                Inventory.Instance.RemoveItems(objectReference, 1);
            }
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (CraftingUIManager.Instance.CraftingTableObject.activeSelf)
            {
                SendToTable();
            }
            else
            {
                objectReference.ExecuteOutcome(Inventory.Instance.gameObject);
            }
        }
    }
}
