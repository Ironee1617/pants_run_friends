using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Question : MonoBehaviour, IPointerDownHandler
{
    RectTransform rtf;
    // Start is called before the first frame update
    void Start()
    {
        rtf = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData ped)
    {
        if(rtf.anchoredPosition3D.x == -2700)
        {
            rtf.anchoredPosition3D = new Vector3(2700, 0, -300);
            gameObject.SetActive(false);
            return;
        }
        rtf.anchoredPosition3D -= new Vector3(1080, 0, 0);
    }
}
