using UnityEngine;

[CreateAssetMenu(fileName = "Food(generic)", menuName = "Items/Food")]
public class FoodSO : ItemSO
{
    
    private int SavedId;

    protected override void Awake()
    {
        ItemId = SavedId;
        base.Awake();
        SavedId = ItemId; 
    }
}