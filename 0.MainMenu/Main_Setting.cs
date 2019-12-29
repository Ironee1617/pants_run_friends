using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_Setting : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Manager_SaveLoad.Instance.Init();
        Manager_SaveLoad.Instance.LoadSetting();

        MaterialSetting();
        GoodsSetting();
    }

    void MaterialSetting()
    {
        Manager_SaveLoad.Instance.CharacterMaterialSetting();
    }

    void GoodsSetting()
    {

    }
}
