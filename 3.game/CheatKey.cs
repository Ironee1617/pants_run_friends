using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheatKey : MonoBehaviour , IPointerDownHandler{

    public void OnPointerDown(PointerEventData ped)
    {
        Manager_Game.Instance.Gold += 10000;
        Manager_Game.Instance.Cash += 10000;
    }
}
