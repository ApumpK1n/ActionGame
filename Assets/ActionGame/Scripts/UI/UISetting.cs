using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISetting : MonoBehaviour
{
    [SerializeField] private Button btnClose;
    [SerializeField] GridLayoutGroup layoutGroup;
    [SerializeField] private RebindKey rebindKeyPrefab; 
    // Start is called before the first frame update
    void Start()
    {
        foreach (RebindKeys key in Enum.GetValues(typeof(RebindKeys)))
        {
            RebindKey rebindKey = Instantiate(rebindKeyPrefab, layoutGroup.transform);
            rebindKey.SetData(key);
        }

        btnClose.onClick.AddListener(OnClose);
    }

    private void OnDestroy()
    {
        btnClose.onClick.RemoveListener(OnClose);
    }


    void OnClose()
    {
        UISystem.Instance.SetUISettingVisible(false);
    }
}


public enum RebindKeys
{
    Up,
    Down,
    Left,
    Right,
    Jump,
    Skill1,
    Skill2,
    Skill3,
    Skill4,
    Bag,
}
