using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOOM_Ordinary : MonoBehaviour, BOOM
{
    public List<GameObject> obstacles;
    public GameObject doubleBomb;
    public BoxCollider boxCollider;
    public BOOM_Protect protector;

    public int scoreIncrease;
    // Use this for initialization

    private void Awake()
    {
        scoreIncrease = 1;
        Manager_Fever.StartFever += FeverTime;
        Manager_Fever.EndFever += EndFeverTime;
        boxCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        
        if (Manager_Fever.fever)
            FeverTime();
        else
            EndFeverTime();
    }

    private void OnDisable()
    {
    }
    public void PushToPool()
    {
        GameObject effect;

        for (int i = 0; i < obstacles.Count; ++i)
        {
            effect = Manager_ObjectPool.Instance.PopFromPool(8);
            effect.transform.position = obstacles[i].transform.position;
            effect.SetActive(true);
            Manager_ObjectPool.Instance.PushToPool(2, obstacles[i].gameObject);
        }
       
        if (doubleBomb)
        {
            BOOM_Ordinary anotherBoom = doubleBomb.GetComponent<BOOM_Ordinary>();
            anotherBoom.obstacles.Clear();
            anotherBoom.doubleBomb = null;

            if (anotherBoom.protector)
            {
                anotherBoom.boxCollider.enabled = true;
                Manager_ObjectPool.Instance.PushToPool(6, anotherBoom.protector.gameObject);
                anotherBoom.protector = null;
            }

            effect = Manager_ObjectPool.Instance.PopFromPool(7);
            effect.transform.position = anotherBoom.transform.position;
            effect.SetActive(true);

            Manager_ObjectPool.Instance.PushToPool(3, doubleBomb);
            doubleBomb = null;
        }

        effect = Manager_ObjectPool.Instance.PopFromPool(7);
        effect.transform.position = transform.position;
        effect.SetActive(true);
        obstacles.Clear();
        protector = null;
        Manager_ObjectPool.Instance.PushToPool(3, this.gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            Role();
            other.GetComponent<Bullet>().PushToPool();
            PushToPool();
            
        }
    }

    public void Role()
    {
        Manager_Sound.Instance.EffectSoundOn();
        Manager_Game.Instance.CurrentScore += scoreIncrease;
        Manager_Fever.Instance.FeverGaugeUp();
        PrintScore();
    }

    void FeverTime()
    {
        scoreIncrease = 2;
    }

    void EndFeverTime()
    {
        scoreIncrease = 1;
    }

    void PrintScore()
    {
        GameObject score = Manager_ObjectPool.Instance.PopFromPool(4);
        Vector3 pos = Camera.main.WorldToScreenPoint(this.transform.position);
        score.transform.position = new Vector3(pos.x, pos.y, score.transform.position.z);
        score.SetActive(true);
    }
}
