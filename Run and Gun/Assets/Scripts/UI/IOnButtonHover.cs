using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IOnButtonHover : MonoBehaviour, IPointerEnterHandler
{
    public AudioSource OnHoverPlay;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        OnHoverPlay.Play();
    }
}
