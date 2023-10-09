public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;  
    } 
    public PlayerBaseState Aim()
    {
        return new PlayerAimState(_context, this);
    }
    public PlayerBaseState Catch()
    {
        return new PlayerCatchState(_context, this);
    }
    public PlayerBaseState Dodge()
    {
        return new PlayerDodgeState(_context, this);
    }
    public PlayerBaseState Equipped()
    {
        return new PlayerEquippedState(_context, this);
    }
    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Hurt()
    {
        return new PlayerHurtState(_context, this);
    }
    public PlayerBaseState Unequipped()     
    {
        return new PlayerUnequippedState(_context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }
}
