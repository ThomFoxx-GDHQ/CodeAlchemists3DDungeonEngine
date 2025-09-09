using System;
using UnityEngine;

[Serializable]
public class ItemStruct 
{
    [SerializeField] private int _id;
    [SerializeField] private int _quantity;

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
