using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIExit : MonoBehaviour, IPointerDownHandler{
    public Store_Gacha sg; 
	public void OnPointerDown(PointerEventData ped)
    {
        if(transform.name == "GachaResult")
        {
            sg.TouchExit();
            transform.parent.gameObject.SetActive(false);
        }
        else if (transform.name == "VideoSkip")
        {
            sg.VideoSkip();
        }
        else
        {
            transform.parent.gameObject.SetActive(false);
        }
            
    }
}
