using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragEventListener : MonoBehaviour, IDragHandler
{
    [SerializeField]
    private UnityEvent<PointerEventData> _onDrag = null;

    public void OnDrag(PointerEventData eventData)
    {
        _onDrag?.Invoke(eventData);
    }
}