using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {
    float time;
    GameObject parent;
    Animator anim;

    private void OnEnable()
    {
        parent = GameObject.Find("GameCanvas").gameObject;
        anim = GetComponentInChildren<Animator>();
        transform.SetParent(parent.transform);
        time = 0.5f;
        anim.Play("Score");

        Text text = GetComponentInChildren<Text>();
        text.text = "" + Manager_Game.Instance.CurrentScore;
        text.color = new Color(Random.Range(0f,1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    private void Update()
    {
        time -= Time.deltaTime;
        if(time <= 0)
        {
            Manager_ObjectPool.Instance.PushToPool(4, this.gameObject);
        }
    }
}
