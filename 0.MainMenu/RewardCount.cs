using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardCount : MonoBehaviour
{
    Text text;
    Button btn;

    int minute;
    int second;
    GameObject particle;

    public GameObject rewardWindow;

    Coroutine cor;
    // Use this for initialization
    void Start()
    {
        text = GetComponentInChildren<Text>();
        particle = GetComponentInChildren<ParticleSystem>().gameObject;
        btn = GetComponent<Button>();
        particle.SetActive(false);


        if (Manager_SaveLoad.Instance.GetSaveData() != 0)
            UTime.Instance.HasConnection((bool _check) => {
                if (_check)
                    cor = StartCoroutine(TimeCheck());
            });
    }

    IEnumerator TimeCheck()
    {
        while (true)
        {
            minute = (int)Manager_Game.Instance.RewardTime / 60;
            second = (int)Manager_Game.Instance.RewardTime % 60;
            if (Manager_Game.Instance.RewardTime > 0)
                text.text = string.Format("{0:00}:{1:00}", minute, second);

            if (Manager_Game.Instance.RewardTime <= 0)
            {
                text.enabled = false;
                particle.SetActive(true);
                btn.enabled = true;
                if (cor != null)
                    StopCoroutine(cor);
            }
            else
            {
                text.enabled = true;
                particle.SetActive(false);
                btn.enabled = false;
            }
            yield return null;
        }
    }

    private void CompareTime()
    {
        DateTime serverTime;
        TimeSpan compareTime;
        UTime.Instance.GetUtcTimeAsync((bool success, string error, DateTime time) =>
            {
                serverTime = time.ToLocalTime();
                compareTime = serverTime - DateTime.Now;
            });
    }

    public void RewardBtn()
    {
        btn.enabled = false;
        UTime.Instance.HasConnection((bool _check)=> {
            if (_check)
            {
                text.enabled = true;
                particle.SetActive(false);

                Manager_SaveLoad.Instance.RewardTime(()=> {
                    Manager_SaveLoad.Instance.SetCash(10);
                    cor = StartCoroutine(TimeCheck());
                    UISetting.Instance.DropCash();
                });
            }
        });
    }
}
