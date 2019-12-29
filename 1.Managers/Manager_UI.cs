using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Manager_UI : MonoBehaviour {
    public GameObject menu;
    public GameObject gameoverPanel;
    public Text highScore;
    public Text currentScore;
    public Text r_gold;
    public Text r_cash;
    public GameObject countDown;
    public GameObject bestScore;
    public Image soundBtn;
    public Sprite soundOffImage;
    public Sprite soundOnImage;

    public delegate void PauseHandler();
    public static event PauseHandler OnPause;

    // Use this for initialization
    void OnEnable () {
        GameOver_Handler.OnGameOver += GameOver;
        if (!Manager_Game.Instance.Sound)
            soundBtn.sprite = soundOffImage;
        else
            soundBtn.sprite = soundOnImage;
    }

    //gameover
    public void GameOver()
    {
        GameOver_Handler.OnGameOver -= GameOver;
        Time.timeScale = 0;

        DisplayScore();
        Reward();
        gameoverPanel.SetActive(true);
    }

    void DisplayScore()
    {
        switch(Manager_Game.Instance.gameLevel)
        {
            case Manager_Game.GameLevel.Easy:
                highScore.text = "" + Manager_Game.Instance.EasyScore;
                if (Manager_Game.Instance.CurrentScore > Manager_Game.Instance.EasyScore)
                {
                    Manager_SaveLoad.Instance.SetScore(Manager_Game.Instance.EasyScore);
                    bestScore.SetActive(true);
                }
                break;
            case Manager_Game.GameLevel.Normal:
                highScore.text = "" + Manager_Game.Instance.NormalScore;
                if (Manager_Game.Instance.CurrentScore > Manager_Game.Instance.NormalScore)
                {
                    Manager_SaveLoad.Instance.SetScore(Manager_Game.Instance.NormalScore);
                    bestScore.SetActive(true);
                }
                break;
            case Manager_Game.GameLevel.Hard:
                highScore.text = "" + Manager_Game.Instance.HardScore;
                if (Manager_Game.Instance.CurrentScore > Manager_Game.Instance.HardScore)
                {
                    Manager_SaveLoad.Instance.SetScore(Manager_Game.Instance.HardScore);
                    bestScore.SetActive(true);
                }
                break;
        }

        currentScore.text = "" + Manager_Game.Instance.CurrentScore;
    }

    void Reward()
    {
        int gold = (int)Manager_Game.Instance.CurrentScore / 10;
        //int cash = (int)Manager_Game.Instance.CurrentScore/10;

        r_gold.text = "" + gold;
        //r_cash.text = "" + cash;

        Manager_SaveLoad.Instance.SetGold(gold);
        //Manager_SaveLoad.Instance.SetCash(cash);
    }

    public void MainMenuBtn()
    {
        Manager_Game.Instance.CurrentScore = 0;
        GameOver_Handler.OnGameOver -= GameOver;
        Time.timeScale = 1;
        SceneManager.LoadScene("0.MainMenu");
    }

    public void PauseBtn()
    {
        if (Time.timeScale != 0)
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ResumeBtn()
    {
        menu.SetActive(false);
        if(!countDown.activeSelf)
            countDown.SetActive(true);
        else
        {
            Time.timeScale = 1;
        }
    }

    public void RestartBtn()
    {
        Manager_Game.Instance.CurrentScore = 0;
        GameOver_Handler.OnGameOver -= GameOver;
        Time.timeScale = 1;
        SceneManager.LoadScene("1.Game");
    }

    public void SoundOnOff()
    {
        if (!Manager_Game.Instance.Sound)
        {
            Manager_Sound.Instance.SoundOn();
            soundBtn.sprite = soundOnImage;
        }
        else
        {
            Manager_Sound.Instance.SoundOff();
            soundBtn.sprite = soundOffImage;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        if(pause)
        {
            if (gameoverPanel.activeSelf)
                return;
            menu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
