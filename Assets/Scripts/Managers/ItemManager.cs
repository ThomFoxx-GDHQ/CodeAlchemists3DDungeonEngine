using System.Collections.Generic;
using UnityEngine;

public class ItemManager 
{
    /*private static int _id = 0;
    public static int ID
    {
        get
        {
            
            _id++;
            return _id;
        }
    }*/

    private static Dictionary<ItemSO, int> itemSo = new Dictionary<ItemSO, int>();
    
    public static int GetOrSetId(ItemSO item, int index)
    {
        //if we already have an ID saved
        if (index != 0)
        {
            //check if it's in the list, if so add it, otherwise update the index
            if (itemSo.ContainsKey(item))
            {
                itemSo[item] = index;
            }
            else
            {
                itemSo.Add(item, index);
            }
            return index;
        }
        
        //check to see if it's already in the list 
        itemSo.TryGetValue(item, out int value);
        //if it returns the default value, means it's not in the list.
        if (value == default)
        {
            value = AddItem(item);
        }

        return value;
    }

    private static int AddItem(ItemSO item)
    {
        var index = 0;
        //probably a better way to do this, (attempt number 6 to get consistent ID that's visible in editor but can't edit)
        //checking stored ID's till it finds one that isn't used  (so we can reuse ID's if we delete an item)
        //When an unused ID is found breaks out of loop.
        for (int i = 1; i < 100; i++)
        {
            if (itemSo.ContainsValue(i)) continue;
            
            index = i;
            itemSo.Add(item, index);
            break;
        }

        //should only get called if we get more then 100 items at this point.
        if (index == 0)
        {
            index = itemSo.Count + 1;
            itemSo.Add(item, index);
        }
        return index;
    }

    public static void RemoveItem(ItemSO item)
    {
        //removes the item from the list so we can reuse the ID.
        itemSo.Remove(item);
    }

    public Item RequestItem(ItemSO item)
    {
        return null;
    }

    public static ItemSO RequestItem(int index)
    {
        foreach (KeyValuePair<ItemSO,int> kvp in itemSo)
        {
            if (kvp.Value == index)
            {
                Debug.Log("Returning Requested Item");
                return kvp.Key;
            }
        }

        Debug.Log("Returning Null on Request Item");
        return null;
    }
}
