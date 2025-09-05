using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class InventoryManager : MonoSingleton<InventoryManager>
{
    private Character _character;
    private GameObject _inventoryPanel;
    private List<InventorySlot> _slots = new List<InventorySlot>();
    [SerializeField] private GameObject _inventoryItem;

    public GameObject InventortyPanel => _inventoryPanel;

    public override void Init()
    {
        InventorySlot[] inventory = GameObject.FindObjectsByType<InventorySlot>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i].IsPanel)
                _inventoryPanel = inventory[i].gameObject;
            else
                _slots.Add(inventory[i]);
        }
        _slots = _slots.OrderBy(x => x.SlotIndex).ToList();
    }

    public void SetCharacter(Character character)
    {
        Debug.Log("Set Character for Inventory");
        _character = character;
        LoadInventoryPanel(_character.GetInventory());
    }

    private void LoadInventoryPanel(ItemStruct[,] items)
    {
        int count = 0;

        Debug.Log($"Load Panel: Count{count}, {items.GetLength(1)}/{items.GetLength(0)}");

        for (int r = 0; r < items.GetLength(0); r++)
        {
            Debug.Log($"Row: {r}");
            for (int c = 0; c < items.GetLength(1); c++)
            {
                Debug.Log($"Column: {c}");
                if (count >= _slots.Count)
                {
                    Debug.Log($"Count is Greater than Slots: {count} > {_slots.Count}");
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
                    Debug.Log("Instantiate Item in UI: ");
                    InventoryItem go = Instantiate(_inventoryItem, _inventoryPanel.transform).GetComponent<InventoryItem>();
                    go.Initialization(itemSO);
                    _slots[count].UpdateSlot(go.gameObject);
                    Debug.Log($"Current Count:{count}");
                }
                count++;
            }
        }
    }

    public void ClearPanel()
    {
        foreach (InventorySlot slot in _slots)
            slot.UpdateSlot(null);
    }

    public void MoveItems(int originalIndex, int targetIndex)
    {
        _character.MoveInventoryItems(SlotConverter(originalIndex), SlotConverter(targetIndex));
    }

    private Vector2Int SlotConverter(int index)
    {
        int x = index / _character.InventoryWidth;
        int y = index % _character.InventoryWidth;
        Debug.Log($"{index} => {x}/{y}");
        return new Vector2Int(x, y);
    }

    public int ItemCount(int slot)
    {
        return _character.GetInventoryInfo(SlotConverter(slot)).Quantity;
    }
}
