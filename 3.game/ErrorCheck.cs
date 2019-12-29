using System;
using UnityEngine;
using UnityEngine.UI;

public class ErrorCheck : MonoBehaviour
{
    private static ErrorCheck _instance;
    public static ErrorCheck Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(ErrorCheck)) as ErrorCheck;
            return _instance;
        }
        set { _instance = value; }
    }

    public GameObject errorPanel;
    public Text text;
    
    public void ErrorConfirm(Action callback)
    {
        try
        {
            callback();
        }
        catch (Exception e)
        {
            errorPanel.SetActive(true);
            text.text = "" + e;
        }
    }

    public void ErrorExit()
    {
        Debug.Log("out");
        Application.Quit();
    }
}
