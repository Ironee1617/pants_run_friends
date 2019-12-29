using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEditor;


#region Class Date
[System.Serializable]
public class Characters
{
    public int characterID;  // 현재 적용 캐릭터
}

[System.Serializable]
public class Themas
{
    public int themaID;
}

[System.Serializable]
public class SettingData
{
    public List<Characters> characters = new List<Characters>();
    public List<Themas> themas = new List<Themas>();
}

[System.Serializable]
public class SaveInformation
{
    public int selectedCharacterID;
    public bool[] enabledCharacter;
    public bool[] enabledPremiumCharacter;
    public int selectedThemaID;
    public bool[] enabledThema;
    public int gold;
    public int cash;
    public int characterPiece;
    public int easyScore;
    public int normalScore;
    public int hardScore;
    public int[] ordinaryGachaStack;
    public int[] premiumGachaStack;
    public bool intro;
    public bool sound;

    public string rewardTime;
    public string adsTime;
}

[System.Serializable]
public class SaveData
{
    public List<SaveInformation> saveInformation = new List<SaveInformation>();
}

[System.Serializable]
public class JsonWrapper
{
    public SaveData saveData;
}

[System.Serializable]
public class JsonWrapperSetting
{
    public SettingData settingData;
}
#endregion

public class Manager_SaveLoad : MonoBehaviour
{
    private static Manager_SaveLoad _instance;
    public static Manager_SaveLoad Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Manager_SaveLoad)) as Manager_SaveLoad;
            return _instance;
        }
        set { _instance = value; }
    }

    string savePath;
    string settingPath;
    string saveFileName;
    string settingFileName;
    System.DateTime currentTime;
    [SerializeField]
    SaveData saveData = new SaveData();
    SaveData tempData = new SaveData();
    SettingData settingData = new SettingData();

    CharacterSelect cs;
    ThemaSelect ts;

    private string key = "dlrfyd7237";

    public Text text;
    // Use this for initialization
    private void Awake()
    {
        saveFileName = "Ang.json";
        savePath = Application.persistentDataPath + "/" + saveFileName;
        cs = GameObject.FindGameObjectWithTag("Setting").GetComponent<CharacterSelect>();
        ts = GameObject.FindGameObjectWithTag("Setting").GetComponent<ThemaSelect>();
        DataLoad();
    }

    public void Init()
    {
        saveFileName = "Ang.json";
        savePath = Application.persistentDataPath + "/" + saveFileName;
        cs = GameObject.FindGameObjectWithTag("Setting").GetComponent<CharacterSelect>();
        ts = GameObject.FindGameObjectWithTag("Setting").GetComponent<ThemaSelect>();
        DataLoad();
    }

    public int GetSaveData()
    {
        return saveData.saveInformation.Count;
    }

    private void OnApplicationQuit()
    {
        DataSave();
        //CloudSaveBtn();
    }
    #region Save&Load Setting
    //게임 업데이트시
    void LoadUpdate()
    {
        tempData.saveInformation.Add(saveData.saveInformation[0]);

        saveData.saveInformation.RemoveAt(0);
        SaveInformation info = new SaveInformation();
        saveData.saveInformation.Add(info);

        saveData.saveInformation[0].selectedCharacterID = tempData.saveInformation[0].selectedCharacterID;
        saveData.saveInformation[0].enabledCharacter = new bool[cs.ordinaryChr.Length];
        saveData.saveInformation[0].enabledCharacter[0] = true;
        for (int i = 1; i < tempData.saveInformation[0].enabledCharacter.Length; ++i)
            saveData.saveInformation[0].enabledCharacter[i] = tempData.saveInformation[0].enabledCharacter[i];

        saveData.saveInformation[0].enabledPremiumCharacter = new bool[cs.premiumChr.Length];
        for (int i = 0; i < tempData.saveInformation[0].enabledPremiumCharacter.Length; ++i)
            saveData.saveInformation[0].enabledPremiumCharacter[i] = tempData.saveInformation[0].enabledPremiumCharacter[i];

        saveData.saveInformation[0].selectedThemaID = tempData.saveInformation[0].selectedThemaID;
        saveData.saveInformation[0].enabledThema = new bool[ts.themas.Length];
        saveData.saveInformation[0].enabledThema[0] = true;
        for (int i = 1; i < tempData.saveInformation[0].enabledThema.Length; ++i)
            saveData.saveInformation[0].enabledThema[i] = tempData.saveInformation[0].enabledThema[i];

        saveData.saveInformation[0].gold = tempData.saveInformation[0].gold;
        saveData.saveInformation[0].cash = tempData.saveInformation[0].cash;
        saveData.saveInformation[0].easyScore = tempData.saveInformation[0].easyScore;
        saveData.saveInformation[0].normalScore = tempData.saveInformation[0].normalScore;
        saveData.saveInformation[0].hardScore = tempData.saveInformation[0].hardScore;

        saveData.saveInformation[0].ordinaryGachaStack = new int[cs.ordinaryChr.Length];
        for (int i = 1; i < tempData.saveInformation[0].ordinaryGachaStack.Length; ++i)
            saveData.saveInformation[0].ordinaryGachaStack[i] = tempData.saveInformation[0].ordinaryGachaStack[i];

        saveData.saveInformation[0].premiumGachaStack = new int[cs.premiumChr.Length];
        for (int i = 1; i < tempData.saveInformation[0].premiumGachaStack.Length; ++i)
            saveData.saveInformation[0].premiumGachaStack[i] = tempData.saveInformation[0].premiumGachaStack[i];

        saveData.saveInformation[0].intro = tempData.saveInformation[0].intro;
        saveData.saveInformation[0].sound = tempData.saveInformation[0].sound;
        saveData.saveInformation[0].rewardTime = tempData.saveInformation[0].rewardTime;

        DataSave();

        //여기
    }

    //게임 맨처음 받아서 실행할 때
    void SaveInit()
    {
        SaveInformation info = new SaveInformation();
        saveData.saveInformation.Add(info);
        saveData.saveInformation[0].selectedCharacterID = 0;
        saveData.saveInformation[0].enabledCharacter = new bool[cs.ordinaryChr.Length];
        saveData.saveInformation[0].enabledCharacter[0] = true;
        for (int i = 1; i < saveData.saveInformation[0].enabledCharacter.Length; ++i)
            saveData.saveInformation[0].enabledCharacter[i] = false;

        saveData.saveInformation[0].enabledPremiumCharacter = new bool[cs.premiumChr.Length];
        for (int i = 0; i < saveData.saveInformation[0].enabledPremiumCharacter.Length; ++i)
            saveData.saveInformation[0].enabledPremiumCharacter[i] = false;

        saveData.saveInformation[0].ordinaryGachaStack = new int[cs.ordinaryChr.Length];
        for (int i = 1; i < saveData.saveInformation[0].ordinaryGachaStack.Length; ++i)
            saveData.saveInformation[0].ordinaryGachaStack[i] = 0;

        saveData.saveInformation[0].premiumGachaStack = new int[cs.premiumChr.Length];
        for (int i = 0; i < saveData.saveInformation[0].premiumGachaStack.Length; ++i)
            saveData.saveInformation[0].premiumGachaStack[i] = 0;

        saveData.saveInformation[0].selectedThemaID = 0;
        saveData.saveInformation[0].enabledThema = new bool[ts.themas.Length];
        saveData.saveInformation[0].enabledThema[0] = true;
        for (int i = 1; i < saveData.saveInformation[0].enabledThema.Length; ++i)
            saveData.saveInformation[0].enabledThema[i] = false;

        saveData.saveInformation[0].gold = 0;
        saveData.saveInformation[0].cash = 0;
        saveData.saveInformation[0].easyScore = 0;
        saveData.saveInformation[0].normalScore = 0;
        saveData.saveInformation[0].hardScore = 0;
        saveData.saveInformation[0].intro = true;
        saveData.saveInformation[0].sound = true;

        saveData.saveInformation[0].rewardTime = "";
        //saveData.saveInformation[0].adsTime = "";
        //여기
    }

    //load시 무조건 초기화
    public void LoadSetting()
    {
        Manager_Game.Instance.CharacterNum = saveData.saveInformation[0].selectedCharacterID;
        Manager_Game.Instance.ThemaNum = saveData.saveInformation[0].selectedThemaID;
        Manager_Game.Instance.Gold = saveData.saveInformation[0].gold;
        Manager_Game.Instance.Cash = saveData.saveInformation[0].cash;
        Manager_Game.Instance.EasyScore = saveData.saveInformation[0].easyScore;
        Manager_Game.Instance.NormalScore = saveData.saveInformation[0].normalScore;
        Manager_Game.Instance.HardScore = saveData.saveInformation[0].hardScore;
        Manager_Game.Instance.Intro = saveData.saveInformation[0].intro;
        Manager_Game.Instance.Sound = saveData.saveInformation[0].sound;
    }

    //save시 무조건 초기화
    void SaveSetting()
    {
        saveData.saveInformation[0].selectedCharacterID = Manager_Game.Instance.CharacterNum;
        saveData.saveInformation[0].selectedThemaID = Manager_Game.Instance.ThemaNum;
        saveData.saveInformation[0].gold = Manager_Game.Instance.Gold;
        saveData.saveInformation[0].cash = Manager_Game.Instance.Cash;
        saveData.saveInformation[0].easyScore = Manager_Game.Instance.EasyScore;
        saveData.saveInformation[0].normalScore = Manager_Game.Instance.NormalScore;
        saveData.saveInformation[0].hardScore = Manager_Game.Instance.HardScore;
        saveData.saveInformation[0].intro = Manager_Game.Instance.Intro;
        saveData.saveInformation[0].sound = Manager_Game.Instance.Sound;
    }
    #endregion

    #region Save&Load
    public void DataSave()
    {
        SaveSetting();
        JsonWrapper wrapper = new JsonWrapper();
        wrapper.saveData = saveData;

        string contents = JsonUtility.ToJson(wrapper, true);
        contents = Encrypt(contents, key);

        File.WriteAllText(savePath, contents);
    }

    public void DataLoad()
    {
        if (!File.Exists(savePath))
        {
            File.Create(savePath).Close();
            SaveInit();
            LoadSetting();
            DataSave();

        }

        string contents = File.ReadAllText(savePath);
        contents = Decrypt(contents, key);
        JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(contents);
        saveData = wrapper.saveData;


        if (File.Exists(savePath))
        {
            //if (saveData.saveInformation[0].enabledCharacter.Length != cs.ordinaryChr.Length ||
            //    saveData.saveInformation[0].)
            //{
            //    LoadUpdate();
            //}
        }
        UTime.Instance.GetUtcTimeAsync((bool success, string error, DateTime time) =>
        {
            if (success)
            {
                currentTime = time.ToLocalTime();
                CalculateTime();
            }
            else
            {
                Debug.LogError(error);
                currentTime = DateTime.MinValue;
            }
        });

        LoadSetting();
        CharacterMaterialSetting();
    }
    IEnumerator JsonLoad()
    {
#if UNITY_EDITOR
        string contents = File.ReadAllText(settingPath);
        JsonWrapperSetting wrapper = JsonUtility.FromJson<JsonWrapperSetting>(contents);
        settingData = wrapper.settingData;
        yield return null;
#elif UNITY_ANDROID
        WWW wwwUrl = new WWW(settingPath);
        yield return wwwUrl;

        string contents = wwwUrl.text;
        JsonWrapperSetting wrapper = JsonUtility.FromJson<JsonWrapperSetting>(contents);
        settingData = wrapper.settingData;
#endif
    }
    #endregion

    #region TimeCalculate
    //게임 종료부터 실행까지 시간 계산
    void CalculateTime()
    {
        if (saveData.saveInformation[0].rewardTime == "")
            return;

        string lastTime = saveData.saveInformation[0].rewardTime;
        DateTime lastDateTime = DateTime.Parse(lastTime);
        TimeSpan compareTime = currentTime.ToLocalTime() - lastDateTime;
        Manager_Game.Instance.RewardTime = 1800 - compareTime.TotalSeconds;
    }
    void CalculateAdsTime()
    {
        if (saveData.saveInformation[0].adsTime == "")
            return;

        string lastTime = saveData.saveInformation[0].adsTime;
        DateTime lastDateTime = DateTime.Parse(lastTime);
        TimeSpan compareTime = currentTime.ToLocalTime() - lastDateTime;
        Manager_Game.Instance.AdsTime = 300 - compareTime.TotalSeconds;
    }

    public void RewardTime(Action callback) {

        UTime.Instance.GetUtcTimeAsync((bool success, string error, DateTime time) =>
        {
            if (success)
            {
                currentTime = time.ToLocalTime();
                TimeSave();
                callback();
            }
            else
            {
                Debug.LogError(error);
                currentTime = DateTime.MinValue;
            }
        });
    }

    public void AdsTime(Action callback)
    {
        UTime.Instance.GetUtcTimeAsync((bool success, string error, DateTime time) =>
        {
            if (success)
            {
                currentTime = time.ToLocalTime();
                AdsTimeSave();
                callback();
            }
            else
            {
                Debug.LogError(error);
                currentTime = DateTime.MinValue;
            }
        });
    }
    

    public void TimeSave()
    {
        saveData.saveInformation[0].rewardTime = currentTime.ToString();
        Manager_Game.Instance.RewardTime = 1800;
        DataSave();
    }

    public void AdsTimeSave()
    {
        saveData.saveInformation[0].adsTime = currentTime.ToString();
        Manager_Game.Instance.AdsTime = 300;
        DataSave();
    }

    //public void OnTimeReceived(bool success, string error, DateTime time)
    //{
    //    if (success)
    //    {
    //        currentTime = time.ToLocalTime();
    //    }
    //    else
    //    {
    //        Debug.LogError(error);
    //        currentTime = DateTime.MinValue;
    //    }
    //}
    #endregion

    #region Character&Thema
    //세이브 데이터를 바탕으로 구매 캐릭터 확인
    public void CharacterMaterialSetting()
    {
        for (int i = 1; i < saveData.saveInformation[0].enabledCharacter.Length; ++i)
        {
            if (saveData.saveInformation[0].enabledCharacter[i])
            {
                cs.ordinaryChr[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = cs.materials[i].GetComponent<MeshRenderer>().material;
            }
            else if (!saveData.saveInformation[0].enabledCharacter[i])
            {
                cs.ordinaryChr[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = cs.materials[0].GetComponent<MeshRenderer>().material;
            }
        }

        for (int i = 0; i < saveData.saveInformation[0].enabledPremiumCharacter.Length; ++i)
        {
            if (saveData.saveInformation[0].enabledPremiumCharacter[i])
            {
                cs.premiumChr[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = cs.premiumMaterials[i].GetComponent<MeshRenderer>().material;
                if (i == 4 || i == 6 || i == 13 || i == 16)
                {
                    cs.premiumChr[i].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = cs.premiumMaterials[i].GetComponent<MeshRenderer>().material;
                    cs.premiumChr[i].transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<MeshRenderer>().material = cs.premiumMaterials[i].GetComponent<MeshRenderer>().material;
                }
            }
            else if (!saveData.saveInformation[0].enabledPremiumCharacter[i])
            {
                cs.premiumChr[i].transform.GetChild(0).GetComponent<MeshRenderer>().material = cs.materials[0].GetComponent<MeshRenderer>().material;
                if (i == 4 || i == 6 || i == 13 || i == 16)
                {
                    cs.premiumChr[i].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = cs.materials[0].GetComponent<MeshRenderer>().material;
                    cs.premiumChr[i].transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<MeshRenderer>().material = cs.materials[0].GetComponent<MeshRenderer>().material;
                }
            }
        }
    }

    //몇번 캐릭터 구매 (일반)
    public void EnableCharacter(int _characterNum)
    {
        saveData.saveInformation[0].enabledCharacter[_characterNum] = true;
        cs.ordinaryChr[_characterNum].transform.GetChild(0).GetComponent<MeshRenderer>().material = cs.materials[_characterNum].GetComponent<MeshRenderer>().material;
        CharacterMaterialSetting();
        DataSave();
    }

    public void EnablePremiumCharacter(int _characterNum)
    {
        saveData.saveInformation[0].enabledPremiumCharacter[_characterNum] = true;
        cs.premiumChr[_characterNum].transform.GetChild(0).GetComponent<MeshRenderer>().material = cs.premiumMaterials[_characterNum].GetComponent<MeshRenderer>().material;
        if (_characterNum == 4 || _characterNum == 6 || _characterNum == 13 || _characterNum == 16)
        {
            cs.premiumChr[_characterNum].transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<MeshRenderer>().material = cs.premiumMaterials[_characterNum].GetComponent<MeshRenderer>().material;
            cs.premiumChr[_characterNum].transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<MeshRenderer>().material = cs.premiumMaterials[_characterNum].GetComponent<MeshRenderer>().material;
        }
        CharacterMaterialSetting();
        DataSave();
    }

    public void EnableThema(int _themaNum)
    {
        Manager_Game.Instance.Gold -= 500;
        saveData.saveInformation[0].gold -= 500;
        saveData.saveInformation[0].enabledThema[_themaNum] = true;
        DataSave();
    }

    //몇번 캐릭터 사용가능한지 여부 확인
    public bool GetEnabledCharacter(int _characterNum)
    {
        if (_characterNum < 0)
            return false;
        return saveData.saveInformation[0].enabledCharacter[_characterNum];
    }

    //몇번 캐릭터 사용가능한지 여부 확인 (특수)
    public bool GetEnabledPremiumCharacter(int _characterNum)
    {
        if (_characterNum >= 0)
            return false;
        return saveData.saveInformation[0].enabledPremiumCharacter[Mathf.Abs(_characterNum + 1)];
    }

    //몇번 테마 사용가능한지 여부 확인
    public bool GetEnabledThema(int _themaNum)
    {
        return saveData.saveInformation[0].enabledThema[_themaNum];
    }

    public void BuyWithCharacterPiece(int _characterNum)
    {
        //if(_characterNum)
    }
    #endregion

    #region Set
    public void SetCharacter(int _characterNum)
    {
        saveData.saveInformation[0].selectedCharacterID = _characterNum;
        DataSave();
    }

    public void SetSound(bool _onoff)
    {
        Manager_Game.Instance.Sound = _onoff;
        saveData.saveInformation[0].sound = _onoff;
        DataSave();
    }

    public void SetIntro(bool _onoff)
    {
        Manager_Game.Instance.Intro = _onoff;
        saveData.saveInformation[0].intro = _onoff;
        DataSave();
    }

    public void SetThema(int _themaNum)
    {
        saveData.saveInformation[0].selectedThemaID = _themaNum;
        DataSave();
    }

    public void SetGold(int _gold)
    {
        Manager_Game.Instance.Gold += _gold;
        saveData.saveInformation[0].gold += _gold;
        DataSave();
    }

    public void SetCash(int _cash)
    {
        Manager_Game.Instance.Cash += _cash;
        saveData.saveInformation[0].cash += _cash;
        DataSave();
    }

    public void SetScore(int _score)
    {
        switch (Manager_Game.Instance.gameLevel)
        {
            case Manager_Game.GameLevel.Easy:
                Manager_Game.Instance.EasyScore = Manager_Game.Instance.CurrentScore;
                saveData.saveInformation[0].easyScore = _score;
                break;
            case Manager_Game.GameLevel.Normal:
                Manager_Game.Instance.NormalScore = Manager_Game.Instance.CurrentScore;
                saveData.saveInformation[0].normalScore = _score;
                break;
            case Manager_Game.GameLevel.Hard:
                Manager_Game.Instance.HardScore = Manager_Game.Instance.CurrentScore;
                saveData.saveInformation[0].hardScore = _score;
                break;
        }
        DataSave();
    }

    public void SetOrdinaryStack(int _chrNum)
    {
        saveData.saveInformation[0].ordinaryGachaStack[_chrNum]++;
        DataSave();
    }

    public void SetPremiumStack(int _chrNum)
    {
        int num = Mathf.Abs(_chrNum) - 1;
        saveData.saveInformation[0].premiumGachaStack[num]++;
        DataSave();
    }

    public int GetOrdinaryStack(int _chrNum)
    {
        return saveData.saveInformation[0].ordinaryGachaStack[_chrNum];
    }

    public int GetPremiumStack(int _chrNum)
    {
        int num = Mathf.Abs(_chrNum) - 1;
        return saveData.saveInformation[0].premiumGachaStack[num];
    }
    #endregion

    #region crypt
    public string Decrypt(string textToDecrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] encryptedData = Convert.FromBase64String(textToDecrypt);
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        byte[] plainText = rijndaelCipher.CreateDecryptor().TransformFinalBlock(encryptedData, 0, encryptedData.Length);
        return Encoding.UTF8.GetString(plainText);
    }

    public string Encrypt(string textToEncrypt, string key)
    {
        RijndaelManaged rijndaelCipher = new RijndaelManaged();
        rijndaelCipher.Mode = CipherMode.CBC;
        rijndaelCipher.Padding = PaddingMode.PKCS7;

        rijndaelCipher.KeySize = 128;
        rijndaelCipher.BlockSize = 128;
        byte[] pwdBytes = Encoding.UTF8.GetBytes(key);
        byte[] keyBytes = new byte[16];
        int len = pwdBytes.Length;
        if (len > keyBytes.Length)
        {
            len = keyBytes.Length;
        }

        Array.Copy(pwdBytes, keyBytes, len);
        rijndaelCipher.Key = keyBytes;
        rijndaelCipher.IV = keyBytes;
        ICryptoTransform transform = rijndaelCipher.CreateEncryptor();
        byte[] plainText = Encoding.UTF8.GetBytes(textToEncrypt);
        return Convert.ToBase64String(transform.TransformFinalBlock(plainText, 0, plainText.Length));
    }
    #endregion

    //public void CloudSaveBtn()
    //{
    //    try
    //    {
    //        JsonWrapper wrapper = new JsonWrapper();
    //        wrapper.saveData = saveData;

    //        string contents = JsonUtility.ToJson(wrapper, true);
    //        contents = Encrypt(contents, key);
    //        Manager_CloudData.Instance.SaveToCloud(contents);
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log(e);
    //    }
    //}

    //public void CloudLoadBtn()
    //{

    //    try
    //    {
    //        Manager_CloudData.Instance.LoadFromCloud((string contents) =>
    //        {
    //            contents = Decrypt(contents, key);
    //            JsonWrapper wrapper = JsonUtility.FromJson<JsonWrapper>(contents);
    //            saveData = wrapper.saveData;
    //        });


    //        //UTime.Instance.GetUtcTimeAsync(OnTimeReceived, CalculateTime);
    //        LoadSetting();
    //        CharacterMaterialSetting();
    //    }
    //    catch(Exception e)
    //    {
    //        Debug.Log(e);
    //    }

        
    //}
}