using UnityEngine;

[CreateAssetMenu(fileName = "Weapon(generic)", menuName = "Items/Weapon")]
public class WeaponSO : ItemSO
{
    
    public int _damage;
    public int _stretgh;
    public ClassType[] _classCanWield;

    [HideInInspector]
    [SerializeField]
    private int SavedId;

    protected override void Awake()
    {
        //if we have a saved ID, update ItemId since it doesn't save
        ItemId = SavedId;
        //call base awake to use ItemId to Add our item + Id, or get a new one if no Id was saved.
        base.Awake();
        //if saved, this should be same number, but if none saved, updated with new ID
        SavedId = ItemId; 
    }
    
}
