using UnityEditorInternal;
using UnityEngine;

public class ItemManager 
{
    private static int _id = 0;
    public static int ID
    {
        get
        {
            _id++;
            return _id;
        }
    }

    public Item RequestItem(ItemSO item)
    {
        return null;
    }


}
