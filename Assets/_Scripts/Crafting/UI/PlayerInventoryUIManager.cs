using AYellowpaper.SerializedCollections;
using Fractura.CraftingSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class PlayerInventoryUIManager : SingletonMonoBehaviour<PlayerInventoryUIManager>
{
    [SerializeField] private List<InventoryItemUI> inventoryItemUIs;

    [SerializeField] private GameObject dropPrefab;
    public GameObject DropPrefab => dropPrefab;

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

    //public void AddItemToInventory(CraftingObject newObject, int count)
    //{
    //    bool looped = false;
    //    foreach (InventoryItemUI item in inventoryItemUIs)
    //    {
    //        if (!item.IsAssigned && !looped)
    //        {
    //            item.AssignItem(newObject, count);
    //            Debug.Log($"{gameObject.name}: Assigning {newObject.name} to {item.gameObject.name}");
    //            looped = true;
    //            //break;
    //        }
    //        else if(item.IsAssigned && item.ObjectReference == newObject)
    //        {
    //            item.AssignItem(newObject, count);
    //            Debug.Log($"{gameObject.name}: Updating {newObject.name}'s count to {count}");
    //            looped = true;
    //        }
    //    }
    //}

    public void AddItemToInventory(CraftingObject newObject, int count)
    {
        // First, check if the object is already in one of the inventory slots.
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (item.IsAssigned && item.ObjectReference == newObject)
            {
                item.AssignItem(newObject, count);
                Debug.Log($"{gameObject.name}: Updating {newObject.name}'s count to {count} in {item.gameObject.name}");
                return; // Stop further processing since we've updated the existing slot.
            }
        }

        // If not found, find the first free slot to assign the object.
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (!item.IsAssigned)
            {
                item.AssignItem(newObject, count);
                Debug.Log($"{gameObject.name}: Assigning {newObject.name} to {item.gameObject.name}");
                return;
            }
        }

        // Optionally, you might want to log a warning if there's no free slot.
        Debug.LogWarning("No free inventory slot available for " + newObject.ObjectName);
    }


    public void UpdateItemCountUI(CraftingObject obj, int count)
    {
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (item.IsAssigned && item.ObjectReference == obj)
            {
                item.AssignItem(obj, count);
                Debug.Log($"{gameObject.name}: Updating {obj.name}'s count to count");
                //looped = true;
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
