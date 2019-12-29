using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro_Setting : MonoBehaviour
{
    GameObject player;
    GameObject playerChild;

    public GameObject curtain;
    public GameObject themaObj;
    public Renderer backGround;
    public GameObject[] activeTrue;
    public GameObject[] objs;
    public GameObject dustPrefab;
    public RuntimeAnimatorController introAnimatorController;

    private GameObject dust;
    private Vector3 characterScale;
    private Vector3 characterAngle;

    // Start is called before the first frame update

    void Start()
    {
        objs = new GameObject[3];
        StartIntro();
        
        //StartCoroutine()
    }

    public void SkipIntro() => EndIntro();

    void EndIntro()
    {
        for (int i = 0; i < activeTrue.Length; ++i)
            activeTrue[i].SetActive(true);

        Destroy(dust);
        Manager_FieldControl.Instance.EndIntro();
        

        if (!Manager_Game.Instance.Intro)
        {
            gameObject.SetActive(false);
            return;
        }

        Destroy(player.GetComponent<Intro_ObstacleMove>());
        Destroy(playerChild.GetComponent<Animator>());
        player.GetComponent<Animator>().enabled = true;
        playerChild.transform.localEulerAngles = characterAngle;
        playerChild.transform.localScale = characterScale;
        for (int i = 0; i < 3; ++i)
        {
            Destroy(objs[i]);
        }
        
        gameObject.SetActive(false);
    }

    void StartIntro()
    {
        if(!Manager_Game.Instance.Intro)
        {
            EndIntro();
            return;
        }

        backGround.material = backGround.transform.GetChild(Manager_Game.Instance.ThemaNum).GetComponent<Renderer>().material;

        player = Manager_ObjectPool.Instance.transform.GetChild(0).GetChild(0).gameObject;
        playerChild = player.transform.GetChild(0).gameObject;

        characterScale = playerChild.transform.localScale;
        characterAngle = playerChild.transform.localEulerAngles;

        dust = Instantiate(dustPrefab, player.transform);
        player.transform.position = new Vector3(-30, 0, -5.5f);

        player.GetComponent<Animator>().enabled = false;
        player.AddComponent<Intro_ObstacleMove>();
        playerChild.gameObject.AddComponent<Animator>();
        playerChild.GetComponent<Animator>().runtimeAnimatorController = introAnimatorController;

        GameObject obj = themaObj.transform.GetChild(Manager_Game.Instance.ThemaNum).gameObject;

        StartCoroutine(Create(obj));
        StartCoroutine(CurtainCall());
    }


    IEnumerator Create(GameObject obj)
    {
        for (int i = 0; i < 3; ++i)
        {
            objs[i] = Instantiate(obj);
            if(i == 2)
                objs[i].transform.position = new Vector3(-28, 0, -10);
            else
                objs[i].transform.position = new Vector3(-30 + (-2 * i), 0, -10);
            objs[i].GetComponent<Animator>().SetInteger("AnimNum", i + 1);
            objs[i].AddComponent<Intro_ObstacleMove>();
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    IEnumerator CurtainCall()
    {
        while(true)
        {
            if(player.transform.position.z >= 9)
            {
                StartCoroutine(MoveCurtain());
                break;
            }
                

            yield return null;
        }
    }

    IEnumerator MoveCurtain()
    {
        while (true)
        {
            if (curtain.transform.position.y >= 4f)
            {
                EndIntro();
                break;
            }

            curtain.transform.Translate((Vector3.forward + Vector3.up) * 6 * Time.deltaTime, Space.World);

            yield return null;
        }
    }
}
