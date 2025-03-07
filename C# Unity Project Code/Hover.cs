using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using UnityEngine.UI;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event Action<Button, bool> HoverEvent;

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        HoverEvent(this.gameObject.GetComponent<Button>(), true);
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        HoverEvent(this.gameObject.GetComponent<Button>(), false);
    }


}
