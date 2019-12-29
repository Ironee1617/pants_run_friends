using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Main_Thema : MonoBehaviour {

    private static Main_Thema _instance;
    public static Main_Thema Instance
    {
        get
        {
            if (!_instance)
                _instance = FindObjectOfType(typeof(Main_Thema)) as Main_Thema;
            return _instance;
        }
    }

    Material material;
    Renderer render;

    public GameObject ts;
    public float speed = 0.04f;
    float offset;

	// Use this for initialization
	void Start () {
        //ts = GameObject.FindGameObjectWithTag("Setting").GetComponent<ThemaSelect>();
        render = GetComponent<Renderer>();
        MaterialChange();
        StartCoroutine(Fallen());
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    IEnumerator Fallen()
    {
        while (true)
        {
            //ts.themas[Manager_Game.Instance.ThemaNum].transform.GetChild(0).GetComponent<Renderer>().material;
            offset += Time.deltaTime * speed;
            if (SceneManager.GetActiveScene().buildIndex == 1)
                render.material.mainTextureOffset = new Vector2(offset, offset);
            else
            {
                speed = Manager_FieldControl.Instance.fallingSpeed;
                render.material.mainTextureOffset = new Vector2(0, offset / 17);
            }
            yield return null;
        }
    }

    public void MaterialChange()
    {
        material = ts.transform.GetChild(Manager_Game.Instance.ThemaNum).GetChild(0).GetComponent<Renderer>().material;
        render.material = material;
    }
}
