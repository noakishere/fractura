using AYellowpaper.SerializedCollections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fractura.CraftingSystem
{
    public class Inventory : SingletonMonoBehaviour<Inventory>
    {
        [SerializedDictionary("Object", "Count")]
        [SerializeField] private SerializedDictionary<CraftingObject, int> items = new SerializedDictionary<CraftingObject, int>();
        public SerializedDictionary<CraftingObject, int> Items => items;
        
        #region Actions
        public event Action<CraftingObject, int> OnItemAdded;
        public event Action<CraftingObject, int> OnItemRemoved;
        #endregion

        private void Start()
        {
            foreach(CraftingObject co in items.Keys)
            {
                PlayerInventoryUIManager.Instance.AddItemToInventory(co, items[co]);
            }
        }

        //private void OnEnable()
        //{
        //    OnItemAdded += AddItem;
        //    OnItemRemoved -= RemoveItem;

        //}

        //private void OnDisable()
        //{
        //    OnItemAdded -= AddItem;
        //    OnItemRemoved -= RemoveItem;
        //}

        public int GetItemCount(CraftingObject item)
        {
            if (item == null)
                return 0;

            if (items.TryGetValue(item, out int count))
                return count;

            return 0;
        }

        public void AddItem(CraftingObject item)
        {
            if (item == null) return;

            if(items.Keys.Count != 6)
            {
                if (items.ContainsKey(item))
                {
                    if(items[item] < 6)
                    {
                        items[item]++;
                    }
                }
                else
                {
                    items[item] = 1;
                }

                //PlayerInventoryUIManager.Instance.AddItemToInventory(item);
                OnItemAdded?.Invoke(item, items[item]);
                Debug.Log($"Added {item.ObjectName} to inventory.");
            }
        }

        public void RemoveItems(CraftingObject item, int quantity)
        {
            if (item == null || quantity <= 0) return;

            if (items.TryGetValue(item, out int currentQuantity))
            {
                currentQuantity -= quantity;
                if (currentQuantity <= 0)
                {
                    items.Remove(item);
                    PlayerInventoryUIManager.Instance.RemoveItem(item);
                }
                else
                {
                    items[item] = currentQuantity;
                    PlayerInventoryUIManager.Instance.UpdateItemCountUI(item, items[item]);
                }

                Debug.Log($"Removed {quantity} of {item.ObjectName} from inventory.");
            }
            else
            {
                Debug.LogWarning($"Attempted to remove {item.ObjectName}, but it was not in inventory.");
            }
        }

        public void RemoveItem(CraftingObject item)
        {
            if (item == null) return;

            if (items.ContainsKey(item))
            {
                // remove quantity
                items[item]--;

                if (items[item] <= 0)
                {
                    items.Remove(item);
                }

                Debug.Log($"Removed {item.ObjectName} from inventory.");
            }
            else
            {
                Debug.LogWarning($"Attempted to remove {item.ObjectName}, but it was not in inventory.");
            }
        }
    }
}
