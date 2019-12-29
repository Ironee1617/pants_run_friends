using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager_Sound : MonoBehaviour
{
    GameObject bgm;
    private static Manager_Sound _instance;
    public static Manager_Sound Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Manager_Sound)) as Manager_Sound;
            return _instance;
        }
    }

    public void BGMOn(int _volume = 1, bool _loop = true)
    {
        if (!Manager_Game.Instance.Sound)
            return;
        bgm = Manager_ObjectPool.Instance.PopFromPool(11);
        AudioSource set = bgm.GetComponent<AudioSource>();
        set.volume = _volume;
        set.loop = _loop;

        bgm.SetActive(true);
    }

    public void EffectSoundOn(int _volume = 1, bool _loop = false)
    {
        if (!Manager_Game.Instance.Sound)
            return;
        GameObject effect = Manager_ObjectPool.Instance.PopFromPool(12);
        AudioSource set = effect.GetComponent<AudioSource>();
        set.volume = _volume;
        set.loop = _loop;

        effect.SetActive(true);
        StartCoroutine(EffectSoundOff(effect));
    }

    public void SoundOff()
    {
        Manager_Game.Instance.Sound = false;

        Manager_ObjectPool.Instance.PushToPool(11, bgm);
    }

    public void SoundOn()
    {
        Manager_Game.Instance.Sound = true;

        BGMOn();
    }

    IEnumerator EffectSoundOff(GameObject _sound)
    {
        yield return new WaitForSeconds(1);
        Manager_ObjectPool.Instance.PushToPool(12, _sound);
    }
}
