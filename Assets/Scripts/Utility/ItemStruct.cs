using UnityEngine;

public class ItemStruct 
{
    private int _id;
    private int _quantity;

    public int ID => _id;
    public int Quantity
    {
        get => _quantity;
        set => _quantity = value;
    }

    public ItemStruct(int id, int quantity)
    {
        _id = id;
        _quantity = quantity;
    }
}
