using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Manager_Ads : MonoBehaviour
{
    private const string android_game_id = "3131274";
    private const string ios_game_id = "3131275";

    private const string rewarded_video_id = "rewardedVideo";

    int minute;
    int second;

    [Header("MainMenuScene")]
    public GameObject panel;
    public Text text;
    public Image adImage;
    public Text timecountText;
    private Coroutine cor;

    [Header("GameScene")]
    public GameObject x2;
    bool looked = false;
    public GameObject lookedPanel;

    void Start()
    {
        Initialize();
        if (SceneManager.GetActiveScene().name == "0.MainMenu")
        {
            UTime.Instance.HasConnection((bool _check) => {
                if (_check)
                    cor = StartCoroutine(TimeCount());
            });
        }
    }

    private void Initialize()
    {
#if UNITY_ANDROID
        Advertisement.Initialize(android_game_id);
#elif UNITY_IOS
        Advertisement.Initialize(ios_game_id);
#endif
    }

    public void ShowRewardedAd()
    {
        if (Advertisement.IsReady(rewarded_video_id))
        {
            if(SceneManager.GetActiveScene().name == "0.MainMenu")
            {
                var options = new ShowOptions { resultCallback = MainMenuResult };

                Advertisement.Show(rewarded_video_id, options);
            }
            else
            {
                if (!looked)
                {
                    var options = new ShowOptions { resultCallback = GameOverResult };

                    Advertisement.Show(rewarded_video_id, options);
                    looked = true;
                }
                else
                    lookedPanel.SetActive(true);
            }
        }
        else
            Debug.Log("Not ready ADs");
    }

    private void GameOverResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Debug.Log("The ad was successfully shown.");
                    int a = Manager_Game.Instance.CurrentScore / 10;
                    Manager_SaveLoad.Instance.SetGold(a);
                    x2.SetActive(true);
                    Debug.Log("후 " + Manager_Game.Instance.Gold);
                    break;
                }
            case ShowResult.Skipped:
                {
                    Debug.Log("The ad was skipped before reaching the end.");

                    // to do ...
                    // 광고가 스킵되었을 때 처리

                    break;
                }
            case ShowResult.Failed:
                {
                    Debug.LogError("The ad failed to be shown.");

                    // to do ...
                    // 광고 시청에 실패했을 때 처리

                    break;
                }
        }
    }

    private void MainMenuResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                {
                    Manager_SaveLoad.Instance.AdsTime(()=> {
                        Manager_SaveLoad.Instance.SetCash(10);
                        panel.SetActive(true);
                        text.text = "보석 10개 획득!";
                        cor = StartCoroutine(TimeCount());
                    });
                    break;
                }
            case ShowResult.Skipped:
                {
                    panel.SetActive(true);
                    text.text = "광고가 스킵되었습니다.";

                    break;
                }
            case ShowResult.Failed:
                {
                    panel.SetActive(true);
                    text.text = "광고 실행 실패.";

                    break;
                }
        }
    }

    private IEnumerator TimeCount()
    {
        while (true)
        {
            minute = (int)Manager_Game.Instance.AdsTime / 60;
            second = (int)Manager_Game.Instance.AdsTime % 60;
            if (Manager_Game.Instance.AdsTime > 0)
                timecountText.text = string.Format("{0:00}:{1:00}", minute, second);

            if (Manager_Game.Instance.AdsTime <= 0)
            {
                timecountText.enabled = false;
                adImage.raycastTarget = true;
                adImage.color = new Color(1f, 1f, 1f);
                if (cor != null)
                    StopCoroutine(cor);
            }
            else
            {
                timecountText.enabled = true;
                adImage.raycastTarget = false;
                adImage.color = new Color(.5f, .5f, .5f);
            }
            yield return null;
        }
    }
}
