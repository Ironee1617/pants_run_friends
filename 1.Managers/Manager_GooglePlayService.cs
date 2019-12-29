using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

public class Manager_GooglePlayService : MonoBehaviour
{
    private static Manager_GooglePlayService _instance;
    public static Manager_GooglePlayService Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Manager_GooglePlayService)) as Manager_GooglePlayService;

            return _instance;
        }
    }
    // Start is called before the first frame update
    //void Awake()
    //{
    //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    //        .EnableSavedGames().Build();

    //    PlayGamesPlatform.InitializeInstance(config);
    //    PlayGamesPlatform.DebugLogEnabled = false;
    //    PlayGamesPlatform.Activate();
    //}

    public void Login()
    {
        Social.localUser.Authenticate((bool success) => { if (!success) Debug.Log("Login Fail"); });
    }

    public bool isLogin
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }

    public void Ads30()
    {
        if (!isLogin)
        {
            Login();
            return;
        }
            
        Social.ReportProgress(GPGSIds.achievement_Ads30, 100.0, (bool success) => {
            if (!success) Debug.Log("Report Fail!");
        });
    }

    public void ShowAchivementUI()
    {
        if (!isLogin)
        {
            Login();
            return;
        }
        Social.ShowAchievementsUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
