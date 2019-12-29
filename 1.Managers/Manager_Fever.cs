using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Manager_Fever : MonoBehaviour
{
    private static Manager_Fever _instance;
    public static Manager_Fever Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Manager_Fever)) as Manager_Fever;
            return _instance;
        }
    }

    public delegate void Fever();
    public static event Fever StartFever;
    public static event Fever EndFever;

    float FeverGauge;
    public Image feverBarImage;
    public Button feverButton;
    public static bool fever;
    public Sprite feverEdge;
    public Image feverEdgeImage;
    public Animator anim;
    public Image feverChild;
    public GameObject feverImage;
    public Sprite feverChildSpriteRed;
    public Sprite feverChildSpriteGreen;


    private Coroutine CFeverTime;
    private GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        InitVariable();
        StartCoroutine(FeverButtonEnabled());
    }

    void InitVariable()
    {
        FeverGauge = 0;
        fever = false;
        feverImage.SetActive(false);
    }

    #region Test
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            FeverOn();

        if (Input.GetKeyDown(KeyCode.LeftAlt))
            FeverAnimationOn();
    }
    #endregion

    public void FeverGaugeUp()
    {
        if (!fever)
        {
            if (FeverGauge < 100)
            {
                FeverGauge += 10;
                feverBarImage.fillAmount = FeverGauge * 0.01f;
                if (FeverGauge == 100)
                    FeverAnimationOn();
            }
        }
    }

    void FeverAnimationOn()
    {
        anim.SetBool("Fever", true);
        feverChild.sprite = feverChildSpriteGreen;
        feverChild.gameObject.SetActive(false);
    }

    void FeverAnimationOff()
    {
        anim.SetBool("Fever", false);
        feverChild.gameObject.SetActive(true);
        feverEdgeImage.sprite = feverEdge;

    }

    public void FeverGaugeDown()
    {
        if (!fever)
        {
            if (FeverGauge < 100)
            {
                FeverGauge -= 20;
                FeverGauge = Mathf.Clamp(FeverGauge, 0, 100);
            }


            feverBarImage.fillAmount = FeverGauge * 0.01f;
        }
        else
            FeverOff();
    }

    //IEnumerator FeverTime()
    //{
    //    while (true)
    //    {
    //        if (FeverGauge <= 0)
    //            FeverOff();

    //        FeverGauge -= Time.deltaTime * 10f;
    //        FeverGauge = Mathf.Clamp(FeverGauge, 0, 100);
    //        feverBarImage.fillAmount = FeverGauge * 0.01f;
    //        yield return null;
    //    }
    //}

    public void FeverOn()
    {
        fever = true;
        FeverAnimationOff();
        feverImage.SetActive(true);
        effect = Manager_ObjectPool.Instance.PopFromPool(10);
        effect.SetActive(true);
        StartFever();
    }

    void FeverOff()
    {
        fever = false;
        FeverGauge = 0;
        feverBarImage.fillAmount = FeverGauge * 0.01f;
        feverImage.SetActive(false);
        feverChild.sprite = feverChildSpriteRed;
        Manager_ObjectPool.Instance.PushToPool(10, effect);
        EndFever();
    }

    IEnumerator FeverButtonEnabled()
    {
        while (true)
        {
            if (FeverGauge == 100 && !fever)
                feverButton.enabled = true;
            else
                feverButton.enabled = false;
            yield return null;
        }
    }
}
