using System;

public class Pawn : Actor
{
    /// <summary>
    /// 针对Pawn的控制器
    /// </summary>
    protected ControllerBase m_Controller;

    public Pawn() : base()
    {
    }

    public ControllerBase GetController()
    {
        return m_Controller;
    }

    public void PossessedBy(ControllerBase controller)
    {
        if (controller == null)
        {
            throw new ArgumentNullException(nameof(controller));
        }

        m_Controller = controller;
        m_Controller.Possess(this);

        OnPossessed();
    }

    public void UnPossessed()
    {
        if (m_Controller != null)
        {
            m_Controller.UnPossess();
        }
        m_Controller = null;

        OnUnPossessed();
    }

    protected override void OnTick(float dt)
    {
        base.OnTick(dt);
    }

    protected virtual void OnPossessed()
    { }

    protected virtual void OnUnPossessed()
    { }
}
