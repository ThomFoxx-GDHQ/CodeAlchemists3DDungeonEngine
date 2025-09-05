using Unity.VisualScripting;
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
            transform.GetChild(0).gameObject.SetActive(true);
            _isEmpty = true;
            if (transform.childCount > 1)
            {
                for (int i = transform.childCount -1;  i > 0; i--)
                {
                    if (transform.GetChild(i).gameObject != _countText.gameObject)
                        Destroy(transform.GetChild(i).gameObject);
                }    
            }
            return;
        }
        Debug.Log("Update Slot is Called");
        if (transform.childCount > 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            item.transform.SetParent(transform, false);
            item.transform.SetAsLastSibling();
            item.transform.localPosition = Vector3.zero;
            item.GetComponent<Image>().raycastTarget = true;
            //update the inventory with new location.
            item.GetComponent<InventoryItem>().UpdateSlotIndex(_slotIndex);
            _isEmpty = false;
            int itemCount = InventoryManager.Instance.ItemCount(_slotIndex);
            if (itemCount > 1)
            {
                _countText.gameObject.SetActive(true);
                _countText.SetText("{0}", itemCount);
            }
            else _countText.gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(true);
            _isEmpty = true;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (_isPanel)
        {
            dropped.GetComponent<InventoryItem>().ReturnToSender();
            return;
        }

        if (_isEmpty)
        {
            InventoryManager.Instance.MoveItems(dropped.GetComponent<InventoryItem>().SlotIndex, _slotIndex);
            UpdateSlot(dropped);
        }
        else
        {
            dropped.GetComponent<InventoryItem>().ReturnToSender();
        }
    }

}
