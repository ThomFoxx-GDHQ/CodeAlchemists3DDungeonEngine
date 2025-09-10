using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    bool _isEmpty = true;
    [SerializeField] GameObject _emptySlotImage;
    [SerializeField] bool _isPanel;
    [SerializeField] int _slotIndex;
    TMP_Text _countText;

    public bool IsEmpty => _isEmpty;
    public bool IsPanel => _isPanel;
    public int SlotIndex => _slotIndex;

    private void Start()
    {
        _countText = GetComponentInChildren<TMP_Text>();
    }

    public void UpdateSlot(GameObject item)
    {
        if (item == null)
        {
            SetChildren(false);

            if (transform.childCount <= 1) return;
            
            for (int i = transform.childCount -1;  i > 0; i--)
            {
                if (transform.GetChild(i).gameObject != _countText.gameObject)
                    Destroy(transform.GetChild(i).gameObject);
            }
            return;
        }
        Debug.Log("Update Slot is Called");
        if (transform.childCount > 0)
        {
            SetChildren(true);

            item.transform.SetParent(transform, false);
            item.transform.SetAsLastSibling();
            item.transform.localPosition = Vector3.zero;
            item.GetComponent<Image>().raycastTarget = true;
            //update the inventory with new location.
            item.GetComponent<InventoryItem>().UpdateSlotIndex(_slotIndex);
            //_isEmpty = false;
            int itemCount = InventoryManager.Instance.ItemCount(_slotIndex);
            if (itemCount >= 1)
            {
                _countText.gameObject.SetActive(true);
                _countText.SetText("{0}", itemCount);
            }
            else
            {
                _countText.gameObject.SetActive(false);
            }
        }
        else
        {
            SetChildren(false);
        }
    }

    private void SetChildren(bool hasItem)
    {
        transform.GetChild(0).gameObject.SetActive(!hasItem);
        _countText.gameObject.SetActive(hasItem);
        _isEmpty = !hasItem;
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (_isPanel)
        {
            dropped.GetComponent<InventoryItem>().ReturnToSender();
            return;
        }

        var moveType = InventoryManager.Instance.MoveItems(dropped.GetComponent<InventoryItem>().SlotIndex, _slotIndex);

        switch (moveType)
        {
            case MoveType.SamePos:
                dropped.GetComponent<InventoryItem>().ReturnToSender();
                return;
            case MoveType.EmptySlot or MoveType.AddingSlot:
                //UpdateSlot(dropped); 
                break;
            case MoveType.SwappingSlot:
                var currentItem = GetComponentInChildren<InventoryItem>();
                var droppedItem = dropped.GetComponent<InventoryItem>();
                var slot = droppedItem.OriginalParent.GetComponent<InventorySlot>();
                currentItem.SentToSender(droppedItem.OriginalParent);
                slot.UpdateSlot(currentItem.gameObject);
                
                Debug.Log("swapping?");
                //UpdateSlot(dropped);
                break;
        }
        UpdateSlot(dropped); 
    }

}
