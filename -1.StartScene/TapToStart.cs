using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToStart : MonoBehaviour
{
    public void StartBtn()
    {
        SceneManager.LoadScene("0.MainMenu");
    }
}
