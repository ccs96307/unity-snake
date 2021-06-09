using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class myButtonEvent : Button
{
    // Button is Pressed
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        gameObject.GetComponentInChildren<Text>().text = "Pressed";
    }

    // Button is released
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        gameObject.GetComponentInChildren<Text>().text = "Released";

    }
}
