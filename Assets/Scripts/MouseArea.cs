using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseArea : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler
{
    public static bool isInside = true;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isInside = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isInside = false;
    }
}
