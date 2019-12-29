using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UISetting : MonoBehaviour {
    public Text gold;
    public Text cash;

    public GameObject dropCash;
    public GameObject bgm;
    public VideoPlayer video;

    private static UISetting _instance;
    public static UISetting Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(UISetting)) as UISetting;
            return _instance;
        }
    }

    // Update is called once per frame
    void Update () {
        gold.text = "" + Manager_Game.Instance.Gold;
        cash.text = "" + Manager_Game.Instance.Cash;

        if (Manager_Game.Instance.Sound)
        {
            bgm.SetActive(true);
            video.SetDirectAudioMute(0, false);
        }
        else
        {
            bgm.SetActive(false);
            video.SetDirectAudioMute(0, true);
        }
            

    }

    public void DropCash()
    {
        StartCoroutine(Drop());
    } 

    IEnumerator Drop()
    {
        dropCash.SetActive(true);
        yield return new WaitForSeconds(3);
        dropCash.SetActive(false);
    }
}
