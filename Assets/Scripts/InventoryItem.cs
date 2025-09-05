using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject _draggingIcon;
    [SerializeField] RectTransform _canvas;
    Transform _originalParent;
    InventorySlot _target;
    ItemSO _itemInfo;
    Image _image;
    int _slotIndex;

    public int SlotIndex => _slotIndex;

    private void Start()
    {
        _canvas = InventoryManager.Instance.InventortyPanel.transform.parent.GetComponent<RectTransform>();
        _draggingIcon = this.gameObject;
        _image = GetComponent<Image>();

        if (_image == null) Debug.Log("_image is Null");
        if (_itemInfo == null) Debug.Log("_itemInfo is Null");
        if (_image.sprite == null) Debug.Log("_image.sprite is Null");
        if (_itemInfo.icon == null) Debug.Log("_itemInfo.icon is Null");

        _image.sprite = _itemInfo.icon;
    }

    public void Initialization(ItemSO item)
    {
        _itemInfo = item;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_draggingIcon.transform.parent.TryGetComponent<InventorySlot>(out InventorySlot slot))
        {
            _originalParent = _draggingIcon.transform.parent;
        }
        _draggingIcon.transform.SetParent(_canvas, false);
        _draggingIcon.transform.SetAsLastSibling();
        //InventorySlot invSlot = _originalParent.GetComponent<InventorySlot>();
        slot.UpdateSlot(null);
        _slotIndex = slot.SlotIndex;
        _draggingIcon.GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var rt = _draggingIcon.GetComponent<RectTransform>();
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvas, eventData.position, eventData.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos;
            rt.rotation = _canvas.rotation;
        }
    }

    public void ReturnToSender()
    {
        Debug.Log("Return to Sender Called");
        _draggingIcon.transform.SetParent(_originalParent);
        _draggingIcon.GetComponent<Image>().raycastTarget = true;
        _draggingIcon.transform.SetAsLastSibling();
        _draggingIcon.transform.localPosition = Vector3.zero;        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _draggingIcon.GetComponent<Image>().raycastTarget = true;
    }

    public void UpdateSlotIndex(int id)
    {

        _slotIndex = id;
    }
}
