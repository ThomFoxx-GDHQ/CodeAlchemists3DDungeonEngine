using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _canvasRect;
    private RectTransform _lastParent;
    private Image _icon;

    private void Start()
    {
        if (_canvasRect == null)
            _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        _icon = GetComponent<Image>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(_canvasRect, eventData.position, eventData.pressEventCamera, out Vector3 globalMousePosition))
        {
            this.transform.position = globalMousePosition;                
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _icon.raycastTarget = false;
        _lastParent = (RectTransform)transform.parent;

        transform.SetParent(_canvasRect);
        transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {     
        if (eventData.pointerEnter?.transform.name == "TossPanel")
        {
            // Replace with real Code for removing from inventory
            // and dropping into 'real' world?

            Debug.Log($"{transform.name} was thrown out!");
            Destroy(this.gameObject);
        }

        if(eventData.pointerEnter?.transform.name == "EmptyItem" && eventData.pointerEnter.transform.parent != _lastParent)
        {
            transform.SetParent(eventData.pointerEnter.transform.parent);
        }
        else transform.SetParent(_lastParent);

        transform.localPosition = Vector3.zero;
        _icon.raycastTarget = true;
    }
}
