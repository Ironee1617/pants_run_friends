using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main_Preferences : MonoBehaviour
{
    public Sprite on;
    public Sprite off;
    public Image intro;
    public Image sound;
    //온오프여부 세이브시키기

    // Start is called before the first frame update
    void Start()
    {
        //Manager_Game.Instance.Intro = true;//지우기
        Init();
    }

    void Init()
    {
        if (Manager_Game.Instance.Intro)
            intro.sprite = on;
        else
            intro.sprite = off;

        if (Manager_Game.Instance.Sound)
            sound.sprite = on;
        else
            sound.sprite = off;
    }

    public void IntroOnOff()
    {
        if (Manager_Game.Instance.Intro)
        {
            Manager_SaveLoad.Instance.SetIntro(false);
            intro.sprite = off;
        }
        else
        {
            Manager_SaveLoad.Instance.SetIntro(true);
            intro.sprite = on;
        }
    }

    public void SoundOnOff()
    {
        if (Manager_Game.Instance.Sound)
        {
            Manager_SaveLoad.Instance.SetSound(false);
            sound.sprite = off;
        }
        else
        {
            Manager_SaveLoad.Instance.SetSound(true);
            sound.sprite = on;
        }
    }
}
