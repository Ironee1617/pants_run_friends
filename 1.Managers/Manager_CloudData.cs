using UnityEngine;
using System;
using System.Collections;
//gpg
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
//for encoding
using System.Text;
//for extra save ui
using UnityEngine.SocialPlatforms;
//for text, remove
using UnityEngine.UI;
public class Manager_CloudData : MonoBehaviour
{

    private static Manager_CloudData instance;

    public static Manager_CloudData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Manager_CloudData>();

                if (instance == null)
                {
                    instance = new GameObject("PlayGameCloudData").AddComponent<Manager_CloudData>();
                }
            }

            return instance;
        }
    }

    public bool isProcessing
    {
        get;
        private set;
    }
    public string loadedData
    {
        get;
        private set;
    }
    private const string m_saveFileName = "game_save_data";
    public bool isAuthenticated
    {
        get
        {
            return Social.localUser.authenticated;
        }
    }
    public void ShowAchivementUI()
    {
        Social.ShowAchievementsUI();
    }

    //private void InitiatePlayGames()
    //{
    //    PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
    //    // enables saving game progress.
    //    .EnableSavedGames()
    //    .Build();

    //    PlayGamesPlatform.InitializeInstance(config);
    //    // recommended for debugging:
    //    PlayGamesPlatform.DebugLogEnabled = false;
    //    // Activate the Google Play Games platform
    //    PlayGamesPlatform.Activate();
    //}

    //private void Awake()
    //{
    //    InitiatePlayGames();
    //}


    public void Login(Text text)
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (!success)
            {
                text.text = "fail";
                Debug.Log("Fail Login");
            }
        });
    }


    private void ProcessCloudData(byte[] cloudData)
    {
        if (cloudData == null)
        {
            Debug.Log("No Data saved to the cloud");
            return;
        }

        string progress = BytesToString(cloudData);
        loadedData = progress;
    }


    //public void LoadFromCloud(Action<string> afterLoadAction)
    //{
    //    if (isAuthenticated && !isProcessing)
    //    {
    //        StartCoroutine(LoadFromCloudRoutin(afterLoadAction));
    //    }
    //    //else
    //    //{
    //    //    Login();
    //    //}
    //}

    //private IEnumerator LoadFromCloudRoutin(Action<string> loadAction)
    //{
    //    isProcessing = true;
    //    Debug.Log("Loading game progress from the cloud.");

    //    ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
    //        m_saveFileName, //name of file.
    //        DataSource.ReadCacheOrNetwork,
    //        ConflictResolutionStrategy.UseLongestPlaytime,
    //        OnFileOpenToLoad);

    //    while (isProcessing)
    //    {
    //        yield return null;
    //    }

    //    loadAction.Invoke(loadedData);
    //}

    //public void SaveToCloud(string dataToSave)
    //{

    //    if (isAuthenticated)
    //    {
    //        loadedData = dataToSave;
    //        isProcessing = true;
    //        ((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(m_saveFileName, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnFileOpenToSave);
    //    }
    //    else
    //    {
    //        //Login();
    //    }
    //}

    //private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    //{
    //    if (status == SavedGameRequestStatus.Success)
    //    {

    //        byte[] data = StringToBytes(loadedData);

    //        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

    //        SavedGameMetadataUpdate updatedMetadata = builder.Build();

    //        ((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metaData, updatedMetadata, data, OnGameSave);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Error opening Saved Game" + status);
    //    }
    //}


    //private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    //{
    //    if (status == SavedGameRequestStatus.Success)
    //    {
    //        ((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("Error opening Saved Game" + status);
    //    }
    //}


    private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            Debug.LogWarning("Error Saving" + status);
        }
        else
        {
            ProcessCloudData(bytes);
        }

        isProcessing = false;
    }

    private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
    {
        if (status != SavedGameRequestStatus.Success)
        {
            Debug.LogWarning("Error Saving" + status);
        }

        isProcessing = false;
    }

    private byte[] StringToBytes(string stringToConvert)
    {
        return Encoding.UTF8.GetBytes(stringToConvert);
    }

    private string BytesToString(byte[] bytes)
    {
        return Encoding.UTF8.GetString(bytes);
    }
}