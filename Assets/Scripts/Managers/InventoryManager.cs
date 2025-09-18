using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private Character _character;
    private GameObject _inventoryPanel;
    private List<InventorySlot> _slots = new List<InventorySlot>();
    [SerializeField] private GameObject _inventoryItem;

    public GameObject InventortyPanel => _inventoryPanel;
    public Character ActiveCharacter => _character;

    public override void Init()
    {
        InventorySlot[] inventory = GameObject.FindObjectsByType<InventorySlot>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var slot in inventory)
        {
            if (slot.IsPanel)
                _inventoryPanel = slot.gameObject;
            else
                _slots.Add(slot);
        }
        _slots = _slots.OrderBy(x => x.SlotIndex).ToList();
    }

    public void SetCharacter(Character character)
    {
        //Debug.Log("Set Character for Inventory");
        _character = character;
        LoadInventoryPanel(_character.GetInventory());
    }

    private void LoadInventoryPanel(ItemStruct[,] items)
    {
        ClearPanel();

        int count = 0;
        
        if (items != null) 
            //Debug.Log($"Load Panel: Count{count}, {items.GetLength(1)}/{items.GetLength(0)}");
      
        for (int r = 0; r < items.GetLength(0); r++)
        {
            //Debug.Log($"Row: {r}");
            for (int c = 0; c < items.GetLength(1); c++)
            {
                //Debug.Log($"Column: {c}");
                if (count >= _slots.Count)
                {
                    //Debug.Log($"Count is Greater than Slots: {count} > {_slots.Count}");
                    return;
                }

                if (items[r, c] == null)
                {
                    count++;
                    continue;
                }

                ItemSO itemSO = ItemManager.RequestItem(items[r, c].ID);
                if (itemSO != null)
                {
                    //Debug.Log("Instantiate Item in UI: ");
                    InventoryItem go = Instantiate(_inventoryItem, _inventoryPanel.transform).GetComponent<InventoryItem>();
                    go.Initialization(itemSO);
                    _slots[count].UpdateSlot(go.gameObject);
                    //Debug.Log($"Current Count:{count}");
                }
                count++;
            }
        }
    }

    public void ClearPanel() => _slots.ForEach(s => s.UpdateSlot(null));
    

    public MoveType MoveItems(int originalIndex, int targetIndex)
    {
        return _character.MoveInventoryItems(SlotConverter(originalIndex), SlotConverter(targetIndex));
    }

    public Vector2Int SlotConverter(int index)
    {
        int x = index / _character.InventoryWidth;
        int y = index % _character.InventoryWidth;
        // Debug.Log($"{index} => {x}/{y}");
        return new Vector2Int(x, y);
    }

    public int ItemCount(int slot)
    {
        return _character.GetInventoryInfo(SlotConverter(slot)).Quantity;
    }
}
