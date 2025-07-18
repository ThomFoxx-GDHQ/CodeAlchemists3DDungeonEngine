using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform _canvasRect;
    private RectTransform _lastParent;
    private Image _icon;
    private TMP_Text _countDisplay;
    [SerializeField] private ItemSO _item;
    [SerializeField] private int _amount;

    private void Start()
    {
        transform.name = _item.itemName;
       
        if (_canvasRect == null)
            _canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        
        _icon = GetComponent<Image>();
        _icon.sprite = _item.icon;

        if (_countDisplay == null)
            _countDisplay = GetComponentInChildren<TMP_Text>();

        _countDisplay.text = _amount.ToString();
    }

    public void ChangeAmount(int amountToAdd)
    {
        //Debug.Log($"Adding {amountToAdd} to {_amount}.");
        _amount += amountToAdd;
        _countDisplay.text = _amount.ToString();
        if (_amount < 0)
            Destroy(this.gameObject);
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
        //Dropping Over Toss Area
        if (eventData.pointerEnter?.transform.name == "TossPanel")
        {
            // Replace with real Code for removing from inventory
            // and dropping into 'real' world?

            Debug.Log($"{transform.name} was thrown out!");
            Destroy(this.gameObject);
        }

        //Dropping Over Empty Slot
        if (eventData.pointerEnter?.transform.name == "EmptyItem" && eventData.pointerEnter.transform.parent != _lastParent)
        {
            transform.SetParent(eventData.pointerEnter.transform.parent);
        }
        //Dropping Over Slot with Matching Item
        else if (eventData.pointerEnter?.transform.name == _item.name &&
            eventData.pointerEnter?.transform.parent != _lastParent)
        {
            eventData.pointerEnter.GetComponent<DraggableItem>().ChangeAmount(_amount);
            Destroy(this.gameObject);
        }
        //Dropping over Slot with non Matching Item
        else if (eventData.pointerEnter?.transform.parent.name == "ItemHolder")
        {
            transform.SetParent(eventData.pointerEnter.transform.parent);
            eventData.pointerEnter.transform.SetParent(_lastParent);
            eventData.pointerEnter.transform.localPosition = Vector3.zero;
        }
        //Dropping anywhere else
        else
            transform.SetParent(_lastParent);

        transform.localPosition = Vector3.zero;
        _icon.raycastTarget = true;
    }
}

//Check if ID is same and is stackable, add together,
//if ID is the same/different/empty but not stackable, swap slots