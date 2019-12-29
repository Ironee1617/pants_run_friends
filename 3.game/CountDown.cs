using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    float count;
    public Animator anim;
    // Start is called before the first frame update

    private void OnEnable()
    {
        Manager_Touch.Instance.buttonTouch = false;
        Time.timeScale = 1;
        Manager_FieldControl.Instance.StopCor();
        count = 4;
        StartCoroutine(Count());
    }

    IEnumerator Count()
    {
        //Time.timeScale = 0;
        while(count > 0)
        {
            count -= Time.deltaTime;
           
            yield return null;
        }

        Manager_Touch.Instance.buttonTouch = true;
        Manager_FieldControl.Instance.StartCor();
        gameObject.SetActive(false);
    }
}
