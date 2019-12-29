using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Touch : MonoBehaviour {
    private static Manager_Touch _instance;
    public static Manager_Touch Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Manager_Touch)) as Manager_Touch;
            return _instance;
        }
    }

    private GameObject player;
    private Transform fireSpot;

    private Animator ani;
    private float maxDis;
    private int curPos;
    int t;

    public Vector3[] pos;
    public bool buttonTouch;

    [Header("Button")]
    public GameObject btnParent;
    public GameObject f_btnParent;

    [SerializeField]
    Button[] f_btns;
    [SerializeField]
    RectTransform[] f_btnsTr;

    private void Start()
    {

        f_btns = new Button[(int)Manager_Game.Instance.gameLevel];
        f_btnsTr = new RectTransform[(int)Manager_Game.Instance.gameLevel];
        for (int i = 0; i < f_btns.Length; ++i)
        {
            f_btns[i] = f_btnParent.transform.GetChild(i).GetComponent<Button>();
            f_btns[i].gameObject.SetActive(true);
            f_btnsTr[i] = f_btnParent.transform.GetChild(i).GetComponent<RectTransform>();
        }

        FeverInit();
    }

    void OnEnable () {
        player = Manager_ObjectPool.Instance.transform.GetChild(0).GetChild(0).gameObject;
        if (!player)
            Debug.LogError("There are no players");
        fireSpot = player.transform.GetChild(0).GetChild(0).GetComponent<Transform>();
        ani = player.GetComponent<Animator>();
        t = Manager_Game.Instance.CharacterNum;

        InitMoveDis();
    }

    private void OnDisable()
    {
        Manager_Fever.StartFever -= FeverTime_ButtonChange;
        Manager_Fever.EndFever -= FeverTime_ButtonChange;
    }

    #region Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move_Left();
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move_Right();
        else if (Input.GetKeyDown(KeyCode.Space))
            Attack();

        if (Input.GetKeyDown(KeyCode.Alpha1))
            FeverTime_MoveButton(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            FeverTime_MoveButton(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            FeverTime_MoveButton(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            FeverTime_MoveButton(3);
    }
    #endregion

    #region NotFever
    public void Move_Left()
    {
        if (curPos != 0 && Time.timeScale != 0 && buttonTouch)
        {
            ani.SetTrigger("Left");
            curPos--;
            player.transform.position = pos[curPos];
        }
    }

    public void Move_Right()
    {
        if (curPos != pos.Length - 1 && Time.timeScale != 0 && buttonTouch)
        {
            ani.SetTrigger("Right");
            curPos++;
            player.transform.position = pos[curPos];
        }
    }

    public void Attack()
    {
        if (Time.timeScale != 0 && buttonTouch)
        {
            ani.SetTrigger("Attack");
            Bullet bullet = Manager_ObjectPool.Instance.PopFromPool(1).GetComponent<Bullet>();
            bullet.transform.localEulerAngles = new Vector3(-90, 0, 0);
            bullet.transform.position = fireSpot.position;
            bullet.gameObject.SetActive(true);
            bullet.Shot();

            //이거 이펙트 옮기기
            if (t < 0)
            {
                GameObject muzzleParticleObj = Instantiate(bullet.GetComponent<Bullet_Effect>().muzzleParticle, fireSpot.position, fireSpot.rotation);
                Destroy(muzzleParticleObj, 1.5f);
            }
        }
    }

    void InitMoveDis()
    {
        pos = new Vector3[(int)Manager_Game.Instance.gameLevel];
        switch (Manager_Game.Instance.gameLevel)
        {
            case Manager_Game.GameLevel.Easy:
                maxDis = 1.8f;
                curPos = 1;
                for (int i = 0; i < pos.Length; ++i)
                    pos[i] = new Vector3((i * 1.8f) - 1.8f, 0, -2.3f);
                break;

            case Manager_Game.GameLevel.Normal:
                maxDis = 2.1f;
                curPos = 1;
                for (int i = 0; i < pos.Length; ++i)
                    pos[i] = new Vector3((i * 1.4f) - 2.1f, 0, -2.3f);
                break;

            case Manager_Game.GameLevel.Hard:
                maxDis = 2.2f;
                curPos = 2;
                for (int i = 0; i < pos.Length; ++i)
                    pos[i] = new Vector3((i * 1.1f) - 2.2f, 0, -2.3f);
                break;
        }

        player.transform.position = pos[curPos];
    }
    #endregion

    #region Fever

    void FeverInit()
    {
        switch (Manager_Game.Instance.gameLevel)
        {
            case Manager_Game.GameLevel.Easy:
                for (int i = 0; i < f_btnsTr.Length; ++i)
                {
                    f_btnsTr[i].anchoredPosition = new Vector3((i - 1) * 360, 250, 0);
                    f_btnsTr[i].sizeDelta = new Vector2(360, 500);
                }
                break;
            case Manager_Game.GameLevel.Normal:
                for (int i = 0; i < f_btnsTr.Length; ++i)
                {
                    f_btnsTr[i].anchoredPosition = new Vector3((i - 1.5f) * 270, 250, 0);
                    f_btnsTr[i].sizeDelta = new Vector2(270, 500);
                }
                break;
            case Manager_Game.GameLevel.Hard:
                for (int i = 0; i < f_btnsTr.Length; ++i)
                {
                    f_btnsTr[i].anchoredPosition = new Vector3((i - 2) * 216, 250, 0);
                    f_btnsTr[i].sizeDelta = new Vector2(216, 500);
                }
                break;
        }

        Manager_Fever.StartFever += FeverTime_ButtonChange;
        Manager_Fever.EndFever += FeverTime_ButtonChange;
    }

    public void FeverTime_MoveButton(int _pos)
    {
        if (Time.deltaTime != 0)
        {
            player.transform.position = pos[_pos];
            curPos = _pos;
            Attack();
        }
    }

    void FeverTime_ButtonChange()
    {
        if (Manager_Fever.fever)
        {
            f_btnParent.SetActive(true);
            btnParent.SetActive(false);
        }
        else
        {
            btnParent.SetActive(true);
            f_btnParent.SetActive(false);
        }
    }
    #endregion
}
