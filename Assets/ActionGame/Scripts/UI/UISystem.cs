using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO:先做个简版的，后续引入UI框架
public class UISystem : DestroyableSingleton<UISystem> 
{
    [SerializeField] private UISetting uiSetting;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetUISettingVisible(bool visible)
    {
        if (visible)
        {
            uiSetting.Open();
        }
        else
        {
            uiSetting.Close();
        }
    }
}
