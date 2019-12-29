using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_WindowCheck : MonoBehaviour
{
    public GameObject gameEscape;

    // Update is called once per frame
    void Update()
    {
        gameEscape.SetActive(false);
    }

    private void OnDisable()
    {
        gameEscape.SetActive(true);
    }
}
