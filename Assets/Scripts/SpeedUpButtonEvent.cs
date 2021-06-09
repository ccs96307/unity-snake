using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class SpeedUpButtonEvent : Button
{
    private float tempSpeed = 0f;
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);
        tempSpeed = Player.moveSpeed;
        Player.moveSpeed *= 1.5f;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);
        Player.moveSpeed = tempSpeed;
    }
}
