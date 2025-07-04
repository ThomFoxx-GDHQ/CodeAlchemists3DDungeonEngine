using UnityEngine;

[CreateAssetMenu(fileName = "Weapon(generic)", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO
{
    private int _itemId;

    public string _name;
    public string _description;
    public Sprite _icon;    
    public int _value;
    public float _weight;
    public int _damage;
    public int _stretgh;
    public ClassType[] _classCanWield;

    private void Awake()
    {
        _itemId = ItemManager.ID;
    }
}
