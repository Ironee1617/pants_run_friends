using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Escape : MonoBehaviour
{
    public GameObject gameEscape;
    public GameObject panel;
    // Update is called once per frame
    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!gameEscape.activeSelf)
                    this.gameObject.SetActive(false);
                else
                    panel.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gameEscape.activeSelf)
            {
                this.gameObject.SetActive(false);
            }
            else
                panel.SetActive(true);
        }
    }
}
