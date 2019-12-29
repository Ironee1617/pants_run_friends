using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject buyThema;
    public GameObject levelChoice;
    public GameObject settingPanel;
    public GameObject licensePanel;
    public GameObject questionPanel;

    public Text e_score;
    public Text n_score;
    public Text h_score;

    private ThemaSelect ts;

    private void Start()
    {
        ts = GameObject.FindGameObjectWithTag("Setting").GetComponent<ThemaSelect>();
    }

    public void StartBtn()
    {
        ScoreSetting();
        if (Manager_SaveLoad.Instance.GetEnabledCharacter(Manager_Game.Instance.CharacterNum) ||
            Manager_SaveLoad.Instance.GetEnabledPremiumCharacter(Manager_Game.Instance.CharacterNum))
        {
            if(Manager_SaveLoad.Instance.GetEnabledThema(Manager_Game.Instance.ThemaNum))
                levelChoice.SetActive(true);
        }
        else if (!Manager_SaveLoad.Instance.GetEnabledCharacter(Manager_Game.Instance.CharacterNum) &&
            !Manager_SaveLoad.Instance.GetEnabledPremiumCharacter(Manager_Game.Instance.CharacterNum))
            Debug.Log("Disenabled Characters");

        if (!Manager_SaveLoad.Instance.GetEnabledThema(Manager_Game.Instance.ThemaNum))
        {

            if (Manager_Game.Instance.Gold >= 500)
            {
                buyThema.transform.GetChild(1).GetComponent<Image>().sprite = buyThema.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite;
                buyThema.transform.GetChild(1).GetComponent<Button>().enabled = true;
            }
            else
            {
                buyThema.transform.GetChild(1).GetComponent<Image>().sprite = buyThema.transform.GetChild(1).GetChild(1).GetComponent<Image>().sprite;
                buyThema.transform.GetChild(1).GetComponent<Button>().enabled = false;
            }
            buyThema.SetActive(true);
        }
    }

    void ScoreSetting()
    {
        e_score.text = "" + Manager_Game.Instance.EasyScore;
        n_score.text = "" + Manager_Game.Instance.NormalScore;
        h_score.text = "" + Manager_Game.Instance.HardScore;
    }

    public void Exit() => Application.Quit();

    public void YesBtn()
    {
        Manager_SaveLoad.Instance.EnableThema(Manager_Game.Instance.ThemaNum);
        buyThema.SetActive(false);
        StartBtn();
    }

    public void NoBtn() => buyThema.SetActive(false);

    public void EasyBtn() => GameStart(Manager_Game.GameLevel.Easy);

    public void NormalBtn() => GameStart(Manager_Game.GameLevel.Normal);

    public void HardBtn() => GameStart(Manager_Game.GameLevel.Hard);

    public void GameStart(Manager_Game.GameLevel _gameLevel)
    {
        Manager_Game.Instance.gameLevel = _gameLevel;
        Manager_SaveLoad.Instance.SetCharacter(Manager_Game.Instance.CharacterNum);
        Time.timeScale = 1;
        SceneManager.LoadScene("1.Game");
    }

    public void SettingBtn() => settingPanel.SetActive(true);

    public void LicenseBtn() => licensePanel.SetActive(true);
    public void QuestionBtn() => questionPanel.SetActive(true);

    
}
