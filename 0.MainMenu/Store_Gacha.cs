using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class Store_Gacha : MonoBehaviour {
    public GameObject store;
    public GameObject NEM;
    public GameObject gachaUI;
    public GameObject charStore;
    public GameObject cashStore;
    public GameObject changeStore;

    public Text printName;
    public Text printPrice;
    public GameObject gold;
    public GameObject cash;
    public GameObject videoSkip;
    public VideoPlayer video;
    public CharacterSelect cs;

    [SerializeField]
    private GameObject chr;

    private Coroutine cor;
	// Use this for initialization

    public void Store_btn() => store.SetActive(true);


    

    public void Gacha_Chr()
    {
        if (Manager_Game.Instance.Gold < 100)
        {
            NEM.SetActive(true);
            return;
        }

        //if(Manager_SaveLoad.Instance.GetSaveData() == 0)
        //{
        //    errorPanel.SetActive(true);
        //    errorText.text = "알 수 없는 오류 발생\n오류코드 xx01";
        //    return;
        //}

        //if (cs == null)
        //{
        //    errorPanel.SetActive(true);
        //    errorText.text = "알 수 없는 오류 발생\n오류코드 xx02";
        //    return;
        //}

        //try
        //{

        //}
        //catch(Exception e)
        //{
        //    video.gameObject.SetActive(false);
        //    errorPanel.SetActive(true);
        //    errorText.text = "" + e;
        //}
        ErrorCheck.Instance.ErrorConfirm(()=> 
        {
            video.gameObject.SetActive(true);
            video.Play();
            Invoke("VideoIsPlaying", (float)video.clip.length);

            RandomChr(cs.ordinaryChr.Length);
            videoSkip.SetActive(true);
            Manager_SaveLoad.Instance.SetGold(-100);
        });
    }

    public void Gacha_Premium()
    {
        if (Manager_Game.Instance.Cash < 100)
        {
            NEM.SetActive(true);
            return;
        }

        ErrorCheck.Instance.ErrorConfirm(() =>
        {
            video.gameObject.SetActive(true);
            video.Play();
            Invoke("VideoIsPlaying", (float)video.clip.length);

            RandomPremiumChr(cs.premiumChr.Length);
            videoSkip.SetActive(true);
            Manager_SaveLoad.Instance.SetCash(-100);
        });
    }

    void RandomChr(int _length)
    {
        int rand = UnityEngine.Random.Range(1, _length);
        printName.gameObject.SetActive(true);
       
        if (Manager_SaveLoad.Instance.GetEnabledCharacter(rand))
        {
            HaveCharacter(rand);
            printName.text = cs.ordinaryChr[rand].transform.GetChild(0).GetComponent<Character_Name>().name + " Get!\n" + Manager_SaveLoad.Instance.GetOrdinaryStack(rand) + "번째 뽑음";
        }
        else
        {
            Manager_SaveLoad.Instance.EnableCharacter(rand);
            printName.text = cs.chrName[rand];
        }

        if (chr != null)
            Destroy(chr);
        chr = Instantiate(cs.ordinaryChr[rand].gameObject);
        chr.transform.SetParent(gachaUI.transform);
        chr.transform.localPosition = new Vector3(0, (chr.transform.localPosition.y / 192) - 100, -200);
        chr.transform.GetChild(0).localScale = new Vector3(chr.transform.GetChild(0).localScale.x / 128, chr.transform.GetChild(0).localScale.y / 128, chr.transform.GetChild(0).localScale.z / 128);
    }

    void RandomPremiumChr(int _length)
    {
        int rand = UnityEngine.Random.Range(-_length, 0);
        int arrNum = Mathf.Abs(rand) - 1;
        printName.gameObject.SetActive(true);
        if (Manager_SaveLoad.Instance.GetEnabledPremiumCharacter(rand))
        {
            HavePremiumCharacter(rand);
            printName.text = cs.premiumChr[arrNum].transform.GetChild(0).GetComponent<Character_Name>().name + " Get!\n" + Manager_SaveLoad.Instance.GetPremiumStack(rand) + "번째 뽑음";
        }
        else
        {
            Manager_SaveLoad.Instance.EnablePremiumCharacter(arrNum);
            printName.text = cs.premiumChr[arrNum].transform.GetChild(0).GetComponent<Character_Name>().name + " Get!\n";
        }
        if (chr != null)
            Destroy(chr);

        chr = Instantiate(cs.premiumChr[arrNum].gameObject);
        chr.transform.SetParent(gachaUI.transform);
        chr.transform.localPosition = new Vector3(0, chr.transform.localPosition.y / 192 - 100, -200);
        chr.transform.GetChild(0).localScale = new Vector3(chr.transform.GetChild(0).localScale.x / 128, chr.transform.GetChild(0).localScale.y / 128, chr.transform.GetChild(0).localScale.z / 128);
    }

    //캐릭터를 뽑았는데 이미 있는 캐릭터를 처리하는 함수
    void HaveCharacter(int _rand)
    {
        Manager_SaveLoad.Instance.SetOrdinaryStack(_rand);
        int stack = Manager_SaveLoad.Instance.GetOrdinaryStack(_rand);
        if (stack <= 3 && stack > 0)
        {
            Manager_SaveLoad.Instance.SetGold(30);
            printPrice.text = "30";
            gold.SetActive(true);
        }
        else if (stack > 3)
        {
            Manager_SaveLoad.Instance.SetCash(10);
            printPrice.text = "10";
            cash.SetActive(true);
        }
        printPrice.gameObject.SetActive(true);
    }

    void HavePremiumCharacter(int _rand)
    {
        Manager_SaveLoad.Instance.SetPremiumStack(_rand);
        int stack = Manager_SaveLoad.Instance.GetPremiumStack(_rand);
        if (stack <= 3 && stack > 0)
        {
            Manager_SaveLoad.Instance.SetCash(10);
            printPrice.text = "10";
            cash.SetActive(true);
        }
        else if (stack > 3)
        {
            Manager_SaveLoad.Instance.SetCash(30);
            printPrice.text = "30";
            cash.SetActive(true);
        }
        printPrice.gameObject.SetActive(true);
    }

    public void TouchExit()
    {
        printName.text = "";
        printPrice.text = "";
        printName.gameObject.SetActive(false);
        printPrice.gameObject.SetActive(false);
        gold.SetActive(false);
        cash.SetActive(false);
        Destroy(chr);
    }

    void VideoIsPlaying()
    {
        //yield return new WaitForSeconds((float)video.clip.length);
        gachaUI.SetActive(true);
        videoSkip.SetActive(false);
        video.Stop();
        video.gameObject.SetActive(false);
    }

    public void VideoSkip()
    {
        CancelInvoke();
        gachaUI.SetActive(true);
        videoSkip.SetActive(false);
        video.Stop();
        video.gameObject.SetActive(false);
    }
    
    public void StoreChange(int _num)
    {
        if(_num == 0)
        {
            cashStore.SetActive(false);
            changeStore.SetActive(false);
            charStore.SetActive(true);
        }
        else if (_num == 1)
        {
            charStore.SetActive(false);
            changeStore.SetActive(false);
            cashStore.SetActive(true);
        }
        else
        {
            cashStore.SetActive(false);
            charStore.SetActive(false);
            changeStore.SetActive(true);
        }
    }

    public void TradeGoldToCash()
    {
        if (Manager_Game.Instance.Gold >= 100)
        {
            Manager_SaveLoad.Instance.SetCash(10);
            Manager_SaveLoad.Instance.SetGold(-100);
        }
        else
            NEM.SetActive(true);
    }

    public void TradeCashToGold()
    {
        if (Manager_Game.Instance.Cash >= 10)
        {
            Manager_SaveLoad.Instance.SetCash(-10);
            Manager_SaveLoad.Instance.SetGold(100);
        }
        else
            NEM.SetActive(true);
    }
}
