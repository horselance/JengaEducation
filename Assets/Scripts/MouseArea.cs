using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseArea : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{
    public static bool IsInside = true;
    public void OnPointerEnter(PointerEventData eventData)
    {
        IsInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        IsInside = false;
    }
}
