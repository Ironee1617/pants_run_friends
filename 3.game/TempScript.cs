using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    private static TempScript _instance;
    public static TempScript Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(TempScript)) as TempScript;
            return _instance;
        }
        set { _instance = value; }
    }


    public GameObject[] a;
    public GameObject b;

    public int thema = 0;
    public int character = 0;
    public Manager_Game.GameLevel gameLevel;

    public bool intro;

    // Start is called before the first frame update
    private void Awake()
    {
        Manager_Game.Instance.ThemaNum = thema;
        Manager_Game.Instance.CharacterNum = character;
        Manager_Game.Instance.Intro = intro;
        Manager_Game.Instance.gameLevel = gameLevel;
    }
    void Start()
    {
        if (!intro)
        {
            b.SetActive(false);
            for (int i = 0; i < a.Length; ++i)
            {
                a[i].SetActive(true);
            }
            Manager_FieldControl.Instance.Test();
        }
        else
        {

        }
    }

}
