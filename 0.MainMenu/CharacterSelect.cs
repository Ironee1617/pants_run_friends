using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CharacterSelect : MonoBehaviour {
    public RectTransform panel;
    public GameObject _parent;
    public GameObject[] chr;
    public GameObject[] ordinaryChr;
    public GameObject[] premiumChr;
    public GameObject[] materials;
    public GameObject[] premiumMaterials;
    public string[] chrName;
    public RectTransform center;

    public Text charName;
    public GameObject badge;

    private RectTransform[] chrRT;
    private int startChr;

    private float[] distance;
    private float[] disReposition;
    private bool dragging = false;
    private int chrDistance;
    private int minChrNum;
    private int chrLength;
    private int ordinaryChrLength;
    private int premiumChrLength;

    private void Awake()
    {
        ordinaryChrLength = _parent.transform.GetChild(0).childCount;
        premiumChrLength = _parent.transform.GetChild(2).childCount;
        chr = new GameObject[ordinaryChrLength + premiumChrLength];
        ordinaryChr = new GameObject[ordinaryChrLength];
        premiumChr = new GameObject[premiumChrLength];
        materials = new GameObject[ordinaryChrLength];
        premiumMaterials = new GameObject[premiumChrLength];
        chrName = new string[ordinaryChrLength + premiumChrLength];

        for (int i = 0; i < ordinaryChrLength; ++i)
        {
            chr[i] = _parent.transform.GetChild(0).GetChild(i).gameObject;
            ordinaryChr[i] = _parent.transform.GetChild(0).GetChild(i).gameObject;
            materials[i] = _parent.transform.GetChild(1).GetChild(i).gameObject;
        }

        for (int i = 0; i < premiumChrLength; ++i)
        {
            chr[ordinaryChrLength + i] = _parent.transform.GetChild(2).GetChild(i).gameObject;
            premiumChr[i] = _parent.transform.GetChild(2).GetChild(i).gameObject;
            premiumMaterials[i] = _parent.transform.GetChild(3).GetChild(i).gameObject;
        }

        chrLength = chr.Length;
        distance = new float[chrLength];
        disReposition = new float[chrLength];
        chrRT = new RectTransform[chrLength];

        for (int i = 0; i < chrRT.Length; ++i)
        {
            chrRT[i] = chr[i].GetComponent<RectTransform>();
        }


        chrDistance = (int)Mathf.Abs(chr[1].GetComponent<RectTransform>().anchoredPosition.x - chr[0].GetComponent<RectTransform>().anchoredPosition.x);

    }

    private void Start()
    {
        string ID = Manager_Game.Instance.CharacterNum.ToString();
        for (int i = 0; i < chrLength; ++i)
        {
            if (chr[i].name == ID)
            {
                startChr = i;
                break;
            }
        }

        for (int i = 0; i < chrLength; ++i)
        {
            chrName[i] = chr[i].GetComponentInChildren<Character_Name>().name;
        }

        panel.anchoredPosition = new Vector2((startChr) * -300, 0);
        StartCoroutine(Silde());
        StartCoroutine(Lerp());
    }

    void LerpToChr(float _position)
    {
        float newX = Mathf.Lerp(panel.anchoredPosition.x, _position, Time.deltaTime * 15f);
        Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

        panel.anchoredPosition = newPosition;
    }

    public void StartDrag() => dragging = true;

    public void EndDrag() => dragging = false;

    IEnumerator Silde()
    {
        while(true)
        {
            for (int i = 0; i < chr.Length; ++i)
            {
                disReposition[i] = center.position.x - chrRT[i].position.x;
                distance[i] = Mathf.Abs(disReposition[i]);

                if (disReposition[i] > 6)
                {
                    float curX = chrRT[i].anchoredPosition.x;
                    float curY = chrRT[i].anchoredPosition.y;

                    Vector2 newAnchoredPos = new Vector2(curX + (chrLength * chrDistance), curY);
                    chrRT[i].anchoredPosition = newAnchoredPos;
                }
                if (disReposition[i] < -6)
                {
                    float curX = chrRT[i].anchoredPosition.x;
                    float curY = chrRT[i].anchoredPosition.y;

                    Vector2 newAnchoredPos = new Vector2(curX - (chrLength * chrDistance), curY);
                    chrRT[i].anchoredPosition = newAnchoredPos;
                }
            }

            float minDistance = Mathf.Min(distance);

            for (int i = 0; i < chr.Length; ++i)
                if (minDistance == distance[i])
                    minChrNum = i;

            {
                Manager_Game.Instance.CharacterNum = int.Parse(chr[minChrNum].name);
                if (Manager_Game.Instance.CharacterNum < 0)
                    badge.SetActive(true);
                else
                    badge.SetActive(false);
            }
            yield return null;
        }
    }

    IEnumerator Lerp()
    {
        while(true)
        {
            charName.text = chrName[minChrNum];
            if (!dragging)
            {
                LerpToChr(-chrRT[minChrNum].anchoredPosition.x);
            }
            yield return null;
        }
    }
}
