using UnityEngine;


public abstract class ItemSO : ScriptableObject
{ 
    //Since this is abstract, instead of using private (only this can edit it)
    //we use protected so anything that derives from it can edit their own version
    //as if it's in their class as a private field
    public int ItemId { get; protected set; }
    
    protected virtual void Awake()
    {
        ResyncData(); 
    }
    
    public void ResyncData()
    {
        ItemId = ItemManager.GetOrSetId(this, ItemId);
    }
}