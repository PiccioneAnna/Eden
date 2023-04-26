using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{  
    [Header("UI")]
    [HideInInspector] public Image image;
    [HideInInspector] public Text countText;

    public Item item;
    public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    public InventoryItem(Item i, int c)
    {
        this.item = i;
        this.count = c;
    }

    public void InitialiseItem(Item newItem)
    {
        item = newItem;
        Debug.Log(newItem);
        image.sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }

    // Drag and Drop
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        countText.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.parent.parent.parent);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        countText.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }
}
