using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    GameObject _draggingIcon;
    [SerializeField] RectTransform _canvas;
    Transform _originalParent;
    InventorySlot _target;

    private void Start()
    {
        _draggingIcon = this.gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _originalParent = _draggingIcon.transform.parent;
        _draggingIcon.transform.SetParent(_canvas, false);
        _draggingIcon.transform.SetAsLastSibling();
        _originalParent.GetComponent<InventorySlot>().UpdateSlot(_draggingIcon);
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
        _draggingIcon.transform.parent = _originalParent;
        _draggingIcon.GetComponent<Image>().raycastTarget = true;
        _draggingIcon.transform.SetAsLastSibling();
        _draggingIcon.transform.position = Vector3.zero;        
    }


}
