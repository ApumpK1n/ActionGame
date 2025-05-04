using System;

/// <summary>
/// 玩家操控角色控制器
/// </summary>
public class PlayerController : ControllerBase
{
    protected Character m_Character;

    // 输入管理

    // 相机管理

    public PlayerController() : base()
    { }


    public Character SpawnCharacter()
    {
        World world = GetWorld();
        if (world == null)
        {
            throw new NullReferenceException(nameof(World));
        }

        Character character = world.SpawnCharacter();
        character.PossessedBy(this);
        character.Enabled = true;
        character.EnableTick = true;

        return character;
    }


    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    protected override void OnDeinitialize()
    {
        base.OnDeinitialize();
    }

    protected override void OnPossess(Pawn pawn)
    {
        Character characterPawn = pawn as Character;
        if (characterPawn == null)
        {
            throw new ArgumentException(string.Format("cannot conver to Character from: {0}", nameof(pawn)));
        }

        m_Character = characterPawn;
    }

    protected override void OnUnPossess()
    {
        base.OnUnPossess();

        m_Character = null;
    }
}
