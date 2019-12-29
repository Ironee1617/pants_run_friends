using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Start : MonoBehaviour {
    Vector3 playerPos;

    public PooledObject characterPool;
    public PooledObject bulletPool;
    public PooledObject obstaclePool;
    public PooledObject boomPool;
    public PooledObject protectorPool;
    public PooledObject bombExpPool;
    public PooledObject obstacleExpPool;
    public PooledObject protectorExpPool;
    public PooledObject bgmPool;
    public PooledObject effectSoundPool;

    private GameObject characters;
    private GameObject bullets;
    private GameObject obstacles;
    private GameObject booms;
    private GameObject protector;
    private GameObject bombExp;
    private GameObject obstacleExp;
    private GameObject protectorExp;
    private GameObject bgm;
    private GameObject effectSound;

    // Use this for initialization
    void Start () {
        characters = transform.GetChild(0).gameObject;
        bullets = transform.GetChild(1).gameObject;
        obstacles = transform.GetChild(2).gameObject;
        booms = transform.GetChild(3).gameObject;
        protector = transform.GetChild(5).gameObject;
        bombExp = transform.GetChild(6).gameObject;
        obstacleExp = transform.GetChild(7).gameObject;
        protectorExp = transform.GetChild(8).gameObject;
        bgm = transform.GetChild(9).gameObject;
        effectSound = transform.GetChild(10).gameObject;

        SetThema();
        SetObject();
    }

    void SetThema()
    {
        int themaNum = Manager_Game.Instance.ThemaNum;
        obstaclePool.itemPrefab = obstacles.transform.GetChild(themaNum).gameObject;
        boomPool.itemPrefab = booms.transform.GetChild(themaNum).gameObject;
        protectorPool.itemPrefab = protector.transform.GetChild(themaNum).gameObject;
        bombExpPool.itemPrefab = bombExp.transform.GetChild(themaNum).gameObject;
        obstacleExpPool.itemPrefab = obstacleExp.transform.GetChild(themaNum).gameObject;
        protectorExpPool.itemPrefab = protectorExp.transform.GetChild(themaNum).gameObject;
        bgmPool.itemPrefab = bgm.transform.GetChild(themaNum).gameObject;
        effectSoundPool.itemPrefab = effectSound.transform.GetChild(themaNum).gameObject;

    }
	
	void SetObject()
    {
        int character = Manager_Game.Instance.CharacterNum;
        for(int i = 0; i < characters.transform.childCount; ++i)
        {
            if (int.Parse(characters.transform.GetChild(i).gameObject.name) == character)
            {
                characterPool.itemPrefab = characters.transform.GetChild(i).gameObject;
                bulletPool.itemPrefab = bullets.transform.GetChild(i).gameObject;
                break;
            }
        }
        GameObject c = Manager_ObjectPool.Instance.PopFromPool(0);
        c.SetActive(true);
    }
}
