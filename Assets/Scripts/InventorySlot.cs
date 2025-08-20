using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    bool _isEmpty = true;
    [SerializeField] GameObject _emptySlotImage;

    public bool IsEmpty => _isEmpty;

    public void UpdateSlot(GameObject item)
    {
        if (transform.childCount > 1)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            item.transform.SetAsLastSibling();
            item.transform.position = Vector3.zero;
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

        if (_isEmpty)
            UpdateSlot(dropped);
        else
        {
            dropped.GetComponent<InventoryItem>().ReturnToSender();
        }
    }

}
