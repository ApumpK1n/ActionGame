
using System;

public class Actor
{
    protected bool m_Enabled = false;
    protected bool m_EnableTick = false;

    protected Level m_InLevel;

    /// <summary>
    /// 所在的level
    /// </summary>
    public Level InLevel { get { return m_InLevel; } }

    public bool EnableTick
    {
        get { return m_EnableTick; } set { m_EnableTick = value; }
    }

    public bool Enabled
    {
        get { return m_Enabled; } set { m_Enabled = value; }
    }

    public Actor()
    {
        m_EnableTick = true;
    }

    public void Initialize()
    {
        OnInitialize();
    }

    public World GetWorld()
    {
        if(m_InLevel == null)
        {
            return null;
        }

        return m_InLevel.OwingWorld;
    }

    public void OnAddToLevel(Level level)
    {
        if (level == null)
        {
            throw new ArgumentException(nameof(level));
        }

        m_InLevel = level;
    }

    public void Tick(float dt)
    {
        if (m_Enabled)
        {
            OnTick(dt);
        }
    }

    public void Deinitialize()
    {
        OnDeinitialize();
    }

    protected virtual void OnInitialize() { }

    protected virtual void OnDeinitialize() { }

    protected virtual void OnTick(float dt) { }
}
