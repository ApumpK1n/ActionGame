using System;
using UnityEngine;

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
        m_CharacterView.OnBind(this);
    }

    public PlayerController GetController()
    {
        return m_PlayerController;
    }

    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
    }

    // public methods
    public void Move(Vector2 direction)
    {
        m_CharacterView.Move(direction);
    }

    public void PlayerAccelerate(bool accelerate)
    {
        m_CharacterView.SetAccelerate(accelerate);
    }

    public void ExecuteCommand(CommandType commandType)
    {
        m_CharacterView.ExecuteCommand(commandType);
    }

    public void ExecuteSkillCommand(int skillSlot)
    {
        m_CharacterView.PerformSkill(skillSlot);
    }
}
