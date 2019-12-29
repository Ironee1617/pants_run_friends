using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemaSelect : MonoBehaviour {
    public GameObject parent;
    public GameObject center;
    public GameObject[] themas;

    public GameObject r_btn;
    public GameObject l_btn;

    private float[] disFromCenter;
    private int selectedThemaNum;
    private RectTransform p_Rect;

    private Coroutine move;
    private Vector2 tp;
    // Use this for initialization
    void Awake () {
        p_Rect = parent.GetComponent<RectTransform>();
        themas = new GameObject[parent.transform.childCount];
        for(int i = 0; i < themas.Length; ++i)
        {
            themas[i] = parent.transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        selectedThemaNum = Manager_Game.Instance.ThemaNum;
        p_Rect.anchoredPosition = new Vector2(-themas[selectedThemaNum].GetComponent<RectTransform>().anchoredPosition.x, p_Rect.anchoredPosition.y);
    }

    private void Update()
    {
        if(selectedThemaNum == 0)
        {
            l_btn.SetActive(false);
        }
        else if (selectedThemaNum == themas.Length - 1)
        {
            r_btn.SetActive(false);
        }
        else
        {
            l_btn.SetActive(true);
            r_btn.SetActive(true);
        }
    }

    public void RightBtn()
    {
        move = StartCoroutine(Moving(selectedThemaNum, "Right"));
        selectedThemaNum += 1;
        Manager_Game.Instance.ThemaNum = selectedThemaNum;
        Main_Thema.Instance.MaterialChange();
    }

    public void LeftBtn()
    {
        move = StartCoroutine(Moving(selectedThemaNum, "Left"));
        selectedThemaNum -= 1;
        Manager_Game.Instance.ThemaNum = selectedThemaNum;
        Main_Thema.Instance.MaterialChange();
    }

    private IEnumerator Moving(int _selectedNum, string _dir)
    {
        float speed = 7;
        if (_dir == "Right")
        {
            tp = new Vector2(-themas[_selectedNum + 1].GetComponent<RectTransform>().anchoredPosition.x, p_Rect.anchoredPosition.y);

            while (p_Rect.anchoredPosition.x > tp.x)
            {
                p_Rect.transform.Translate(Vector2.left * Time.deltaTime * speed);
                yield return null;
            }
        }
        else
        {
            tp = new Vector2(-themas[_selectedNum - 1].GetComponent<RectTransform>().anchoredPosition.x, p_Rect.anchoredPosition.y);
            while (p_Rect.anchoredPosition.x < tp.x)
            {
                p_Rect.transform.Translate(Vector2.right * Time.deltaTime * speed);
                yield return null;
            }
            
        }
    }
}
