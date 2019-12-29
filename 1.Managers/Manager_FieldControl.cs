using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_FieldControl : MonoBehaviour {
    private static Manager_FieldControl _instance;
    public static Manager_FieldControl Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Manager_FieldControl)) as Manager_FieldControl;
            return _instance;
        }
    }

    public Manager_Game.GameLevel gameLevel;
    public float fallingSpeed; // 블록 속도

    private Vector3[] floorPos;
    
    private int boomRandomPos;
    private GameObject instance; //블록 상위 목록
    private double moveDis = 0; //블록 이동 거리
    private Vector3 scale;
    private float dis;

    public GameObject countDown;

    Coroutine fallen;
    Coroutine create;
    Coroutine speedUp;
    void Start () {
        gameLevel = Manager_Game.Instance.gameLevel;
        floorPos = new Vector3[(int)gameLevel];
        LevelSetting();
        fallingSpeed = 2; //이것도 난이도에 따라 다를지 말지는 아직모름
        instance = GameObject.FindGameObjectWithTag("Instance");
    }

    public void EndIntro()
    {
        countDown.SetActive(true);
        Manager_Sound.Instance.BGMOn();
    }

    public void StartCor()
    {
        fallen = StartCoroutine(Fallen());
        create = StartCoroutine(Create());
        speedUp = StartCoroutine(SpeedUP());
    }

    public void StopCor()
    {
        if (fallen == null)
        {
            return;
        }
        StopCoroutine(fallen);
        StopCoroutine(create);
        StopCoroutine(speedUp);
    }

    // 씬1 테스트용 함수
    public void Test()
    {
        StartCoroutine(Fallen());
        StartCoroutine(Create());
        StartCoroutine(SpeedUP());
    }

    bool DoubleBomb()
    {
        int a = Random.Range(0, 9);
        if (a == 1)
            return true;
        else
            return false;
    }

    void CreateFloor()
    {
        bool doubleBomb = DoubleBomb();
        GameObject[] prefab = new GameObject[(int)gameLevel];
        boomRandomPos = Random.Range(0, prefab.Length);
        if (doubleBomb)
        {
            int prevPos = boomRandomPos;
            prefab[prevPos] = Manager_ObjectPool.Instance.PopFromPool(3);
            prefab[prevPos].transform.localPosition = floorPos[prevPos];
            prefab[prevPos].transform.localScale = scale;
            prefab[prevPos].gameObject.SetActive(true);
            do
            {
                boomRandomPos = Random.Range(0, prefab.Length);
            } while (prevPos == boomRandomPos);

            for (int i = 0; i < prefab.Length; ++i)
            {
                if (i == prevPos) continue;
                if (i == boomRandomPos)
                {
                    prefab[i] = Manager_ObjectPool.Instance.PopFromPool(3);
                    prefab[i].transform.localPosition = floorPos[i];
                    prefab[i].transform.localScale = scale;
                    prefab[i].gameObject.SetActive(true);
                    continue;
                }
                prefab[i] = Manager_ObjectPool.Instance.PopFromPool(2);
                prefab[i].transform.localPosition = floorPos[i];
                prefab[i].transform.localScale = scale;
                prefab[i].gameObject.SetActive(true);
            }

            BOOM_Ordinary boom1 = prefab[prevPos].GetComponent<BOOM_Ordinary>();
            for (int i = 0; i < prefab.Length; ++i)
            {
                if (i == prevPos)
                    continue;
                else if (i == boomRandomPos)
                {
                    boom1.doubleBomb = prefab[i];
                    continue;
                }
                boom1.obstacles.Add(prefab[i]);
            }

            BOOM_Ordinary boom2 = prefab[boomRandomPos].GetComponent<BOOM_Ordinary>();
            for (int i = 0; i < prefab.Length; ++i)
            {
                if (i == boomRandomPos)
                    continue;
                else if (i == prevPos)
                {
                    boom2.doubleBomb = prefab[i];
                    continue;
                }
                boom2.obstacles.Add(prefab[i]);
            }

            boom1.boxCollider.enabled = true;
            boom2.boxCollider.enabled = true;
            CreateProtector(boom1);
        }
        else
        {
            for (int i = 0; i < prefab.Length; ++i)
            {
                if (i == boomRandomPos)
                {
                    prefab[i] = Manager_ObjectPool.Instance.PopFromPool(3);
                    prefab[i].transform.localPosition = floorPos[i];
                    prefab[i].transform.localScale = scale;
                    prefab[i].gameObject.SetActive(true);
                    continue;
                }
                prefab[i] = Manager_ObjectPool.Instance.PopFromPool(2);
                prefab[i].transform.localPosition = floorPos[i];
                prefab[i].transform.localScale = scale;
                prefab[i].gameObject.SetActive(true);
            }

            //같이 터질 장애물들을 BOOM class안에 list에 추가
            BOOM_Ordinary boom = prefab[boomRandomPos].GetComponent<BOOM_Ordinary>();
            for (int i = 0; i < prefab.Length; ++i)
            {
                if (i == boomRandomPos)
                    continue;
                boom.obstacles.Add(prefab[i]);
            }
        }

        for(int i = 0; i < prefab.Length; ++i)
        {
            prefab[i].transform.parent = instance.transform;
        }
    }

    void CreateProtector(BOOM_Ordinary _bomb)
    {
        _bomb.boxCollider.enabled = false;
        
        BOOM_Protect protector = Manager_ObjectPool.Instance.PopFromPool(6).GetComponent<BOOM_Protect>();
        Vector3 pos = protector.initPos;
        Vector3 rot = Vector3.zero;

        protector.transform.parent = _bomb.transform;
        protector.transform.localPosition = pos;
        protector.transform.localEulerAngles = rot;
        protector.bomb = _bomb;
        protector.transform.localScale = new Vector3(1.1f, 1.1f, 1);

        _bomb.protector = protector;

        protector.gameObject.SetActive(true);
    }

    // 블록 z축 크기 1.3정도

    IEnumerator Fallen()
    {
        while (true)
        {
            instance.transform.Translate(Vector3.back * Time.deltaTime * fallingSpeed, Space.Self);
            moveDis += Time.deltaTime * fallingSpeed;
            yield return null;
        }
    }

    IEnumerator Create()
    {
        while (true)
        {
            yield return new WaitUntil(() => moveDis >= dis);
            CreateFloor();
            moveDis = 0;
        }
    }

    IEnumerator SpeedUP()
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            fallingSpeed += 0.2f;
        }
    }

    void LevelSetting()
    {
        switch(gameLevel)
        {
            case Manager_Game.GameLevel.Easy:
                dis = 1.4f;
                scale = new Vector3(100, 100, 100);
                break;

            case Manager_Game.GameLevel.Normal:
                dis = 1.3f;
                scale = new Vector3(75, 75, 100);
                break;

            case Manager_Game.GameLevel.Hard:
                dis = 1.1f;
                scale = new Vector3(60, 50, 70);
                break;
        }

        for (int i = 0; i < floorPos.Length; ++i)
            switch (gameLevel)
            {
                case Manager_Game.GameLevel.Easy:
                    floorPos[i] = new Vector3((i * 1.8f) - 1.8f, 0, 10f);
                    break;

                case Manager_Game.GameLevel.Normal:
                    floorPos[i] = new Vector3((i * 1.4f) - 2.1f, 0, 10f);
                    break;

                case Manager_Game.GameLevel.Hard:
                    floorPos[i] = new Vector3((i * 1.1f) - 2.2f, 0, 10f);
                    break;
            }
    }
}