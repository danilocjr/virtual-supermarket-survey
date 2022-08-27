using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MDXButton : Button
{
    UnityAction _pressedAction;
    UnityAction _releasedAction;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        _pressedAction?.Invoke();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        _releasedAction?.Invoke();
    }

    public void OnPressed(UnityAction action)
    {
        _pressedAction = action;
    }

    public void OnReleased(UnityAction action)
    {
        _releasedAction = action;
    }
}
