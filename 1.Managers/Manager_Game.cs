using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Manager_Game : MonoBehaviour {
    private static Manager_Game _instance;
    public static Manager_Game Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Manager_Game)) as Manager_Game;
            return _instance;
        }
        set { _instance = value; }
    }

    private int _gold;
    public int Gold
    {
        get { return _gold; }
        set { _gold = value; }
    }

    private int _cash;
    public int Cash
    {
        get { return _cash; }
        set { _cash = value; }
    }

    private int _characterNum;
    public int CharacterNum
    {
        get { return _characterNum; }
        set { _characterNum = value; }
    }

    private int _themaNum;
    public int ThemaNum
    {
        get { return _themaNum; }
        set { _themaNum = value; }
    }

    private int _easyScore;
    public int EasyScore
    {
        get { return _easyScore; }
        set { _easyScore = value; }
    }

    private int _normalScore;
    public int NormalScore
    {
        get { return _normalScore; }
        set { _normalScore = value; }
    }

    private int _hardScore;
    public int HardScore
    {
        get { return _hardScore; }
        set { _hardScore = value; }
    }

    private int _currentScore;
    public int CurrentScore
    {
        get { return _currentScore; }
        set { _currentScore = value; }
    }

    private bool _intro;
    public bool Intro
    {
        get { return _intro; }
        set { _intro = value; }
    }

    private bool _sound;
    public bool Sound
    {
        get { return _sound; }
        set { _sound = value; }
    }

    private double _rewardTime;
    public double RewardTime
    {
        get { return _rewardTime; }
        set { _rewardTime = value; }
    }

    private double _adsTime;
    public double AdsTime
    {
        get { return _adsTime; }
        set { _adsTime = value; }
    }

    private int _characterPiece;
    public int CharacterPiece
    {
        get { return _characterPiece; }
        set { _characterPiece = value; }
    }

    public enum GameLevel
    {
        None,
        Nope,
        Empty,
        Easy,
        Normal,
        Hard
    }
    public GameLevel gameLevel;

    private void Awake()
    {
        Screen.SetResolution(1080, 1920, true);
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if(RewardTime > 0)
            RewardTime -= Time.deltaTime;

        if (AdsTime > 0)
            AdsTime -= Time.deltaTime;
    }

    
}
