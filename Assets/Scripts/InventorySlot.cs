using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    bool _isEmpty = true;
    [SerializeField] GameObject _emptySlotImage;
    [SerializeField] bool _isPanel;

    public bool IsEmpty => _isEmpty;
    public bool IsPanel => _isPanel;

    public void UpdateSlot(GameObject item)
    {
        if (item == null)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            _isEmpty = true;
            if (transform.childCount > 1)
            {
                for (int i = transform.childCount -1;  i > 0; i--) 
                    Destroy(transform.GetChild(i).gameObject);
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
            _isEmpty = false;
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
            UpdateSlot(dropped);
        else
        {
            dropped.GetComponent<InventoryItem>().ReturnToSender();
        }
    }

}
