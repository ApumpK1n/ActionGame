using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pawn : Actor
{
    protected PlayerController m_PlayerController;

    public Pawn() : base()
    {
    }

    public void PossessedBy(PlayerController playerController)
    {
        if (m_PlayerController == null)
        {
            throw new ArgumentNullException(nameof(m_PlayerController));
        }

        m_PlayerController = playerController;
        m_PlayerController.Possess(this);
    }

    public void UnPossessed()
    {
        if (m_PlayerController != null)
        {
            m_PlayerController.UnPossess();
        }
        m_PlayerController = null;
    }

    protected override void OnTick(float dt)
    {
        base.OnTick(dt);
    }
}
