using UnityEngine;

public abstract class Item : MonoBehaviour
{
    protected string _name;
    protected string _description;
    protected Sprite _icon;
    protected int _itemID;
    protected int _value;
    protected float _weight;

    public string Name => _name;
    public string Description => _description;
    public int ItemID => _itemID;
    public int Value => _value;
    public float Weight => _weight;

}

public class Weapon : Item
{
    int _damage;
    int _strength;
    ClassType[] _classCanWield;

    public Weapon (int itemID, string name,string description, Sprite icon, int value, float weight, int damage, int strength, ClassType[] classCanWield)
    {
        _itemID = itemID;
        _name = name;
        _description = description;
        _icon = icon;
        _value = value;
        _weight = weight;
        _damage = damage;
        _strength = strength;
        _classCanWield = classCanWield;
    }
}

public class Food : Item
{
    int _foodValue;

    public Food(int itemID, string name, string description, Sprite icon, int value, float weight, int foodValue)
    {
        _itemID = itemID;
        _name = name;
        _description = description;
        _icon = icon;
        _value = value;
        _weight = weight;
        _foodValue = foodValue;            
    }
}

public class Potion : Item
{
    int _potionValue;
    Stats _stat;

    public Potion (int itemID, string name, string description, Sprite icon, int value, float weight, int potionValue, Stats stat)
    {
        _itemID = itemID;
        _name = name;
        _description = description;
        _icon = icon;
        _value = value;
        _weight = weight;
        _potionValue = potionValue;
        _stat = stat;
    }
}

public class Armor : Item
{
    int _defense;

    public Armor(int itemID, string name, string description, Sprite icon, int value, float weight, int defense)
    { 
        _itemID = itemID;
        _name = name;
        _description = description;
        _icon = icon;
        _value = value;
        _weight = weight;
        _defense = defense;
    }
}

public class Key : Item
{
    int _lockId;

    public Key(int itemID, string name, string description, Sprite icon, int value, float weight, int lockId)
    {
        _itemID = itemID;
        _name = name;
        _description = description;
        _icon = icon;
        _value = value;
        _weight = weight;
        _lockId = lockId;
    }
}