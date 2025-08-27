using System.Collections.Generic;
using UnityEngine;

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
        foreach (InventorySlot slot in inventory)
        {
            if (slot.IsPanel)
            {
                _inventoryPanel = slot.gameObject;                
            }
            else
            {
                _slots.Add(slot);
            }
        }
    }

    public void SetCharacter(Character character)
    {
        _character = character;
        LoadInventoryPanel(_character.GetInventory());
    }

    private void LoadInventoryPanel(ItemStruct[,] items)
    {
        int count = 0;

        for (int r = 0; r < items.GetLength(0); r++)
        {
            for (int c = 0; c < items.GetLength(1); c++)
            {
                if (count >= _slots.Count) return;

                ItemSO itemSO = ItemManager.RequestItem(items[r, c].ID);
                if (itemSO != null)
                {
                    InventoryItem go = Instantiate(_inventoryItem, _inventoryPanel.transform).GetComponent<InventoryItem>();
                    go.Initialization(itemSO);
                    _slots[count].UpdateSlot(go.gameObject);
                }
                count++;
            }
        }

    }
}
