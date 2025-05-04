using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 玩家角色（逻辑上的）
/// </summary>
public class Character : Pawn
{
    /// <summary>
    /// 玩家控制的角色显示层
    /// </summary>
    protected ICharacterView m_CharacterView;

    public Character() : base()
    { }

    public void BindCharacterView(ICharacterView characterView)
    {
        if(characterView == null)
        {
            throw new ArgumentNullException(nameof(characterView));
        }

        m_CharacterView = characterView;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
    }
}
