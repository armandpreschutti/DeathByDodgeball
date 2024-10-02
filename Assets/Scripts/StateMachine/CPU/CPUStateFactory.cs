using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUStateFactory
{
    CPUBrain _context;

    public CPUStateFactory(CPUBrain currentContext)
    {
        _context = currentContext;
    }

    public CPUBaseState Active()
    {
        return new CPUActiveState(_context, this);
    }

    public CPUBaseState Idle()
    {
        return new CPUIdleState(_context, this);
    }

    public CPUBaseState Patroling()
    {
        return new CPUPatrolState(_context, this);
    }

    public CPUBaseState BallSearching()
    {
        return new CPUBallSearchingState(_context, this);
    }

    public CPUBaseState EnemySearching()
    {
        return new CPUEnemySearchingState(_context, this);
    }

    public CPUBaseState Attacking()
    {
        return new CPUAttackingState(_context, this);
    }
    
    public CPUBaseState Dodging()
    {
        return new CPUDodgeState(_context, this);
    }

    public CPUBaseState Catching()
    {
        return new CPUCatchingState(_context, this);
    }

    public CPUBaseState Death()
    {
        return new CPUDeathState(_context, this);
    }

}


