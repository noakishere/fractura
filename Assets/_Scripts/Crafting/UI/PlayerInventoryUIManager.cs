using AYellowpaper.SerializedCollections;
using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerInventoryUIManager : SingletonMonoBehaviour<PlayerInventoryUIManager>
{
    [SerializeField] private List<InventoryItemUI> inventoryItemUIs;

    //[SerializeField] private SerializedDictionary<InventoryItemUI, CraftingObject> inventoryObjects;

    private void OnEnable()
    {
        //CraftingManager.Instance.OnCraftingCompleted += AddItemToInventory;
        Inventory.Instance.OnItemAdded += AddItemToInventory;
    }

    private void OnDisable()
    {
        //CraftingManager.Instance.OnCraftingCompleted -= AddItemToInventory;
        Inventory.Instance.OnItemAdded -= AddItemToInventory;
    }

    public void AddItemToInventory(CraftingObject newObject)
    {
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (!item.IsAssigned)
            {
                item.AssignItem(newObject);
                Debug.Log($"{gameObject.name}: Assigning {newObject.name} to {item.gameObject.name}");
                break;
            }
        }
    }

    public void FreeObject(CraftingObject newObject)
    {
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (item.IsAssigned && item.ObjectReference == newObject)
            {
                item.ReturnFromTable();
                break;
            }
        }
    }

    public void FreeAllObjects()
    {
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            item.ReturnFromTable();
            break;
        }
    }

    public void RemoveItem(CraftingObject newObject)
    {
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (item.IsAssigned && item.ObjectReference == newObject)
            {
                item.EmptyItem();
                Debug.Log($"{gameObject.name}: Removing {newObject.name} from inventory");
                break;
            }
        }
    }
}
