public class ControllerBase: Actor
{
    protected Pawn m_Pawn;

    public Pawn Pawn {  get { return m_Pawn; } }

    public ControllerBase() { }

    public void Possess(Pawn pawn)
    {
        if (pawn == null)
        {
            throw new System.ArgumentNullException(nameof(pawn));
        }

        m_Pawn = pawn;

        OnPossess(pawn);
    }

    public void UnPossess()
    {
        m_Pawn = null;

        OnUnPossess();
    }

    protected virtual void OnPossess(Pawn pawn)
    { }

    protected virtual void OnUnPossess()
    { }
}
