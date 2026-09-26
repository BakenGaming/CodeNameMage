using System.Collections.Generic;
using UnityEngine;

//make this not a monobehavior and make GameManager hold the reference to it
public class InventoryManager: MonoBehaviour
{
    public List<InventoryItem> inventory = new List<InventoryItem>();
    private Dictionary<ItemData, InventoryItem> itemDictionary = new Dictionary<ItemData, InventoryItem>();

    private void OnEnable()
    {
        GameManager.i.inventoryManager = this;
        Initialize();
    }
    public void Initialize()
    {
        Item.OnItemCollected += Add;
    }

    private void OnDisable()
    {
        Item.OnItemCollected -= Add;
    }
    public void Add(ItemData itemData)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.AddToStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(itemData);
            inventory.Add(newItem);
            itemDictionary.Add(itemData, newItem);
        }
    }
    public void Remove(ItemData itemData)
    {
        if(itemDictionary.TryGetValue(itemData, out InventoryItem item))
        {
            item.RemoveFromStack();
            if(item.stackSize == 0)
            {
                inventory.Remove(item);
                itemDictionary.Remove(itemData);
            }
        }
    }
}
