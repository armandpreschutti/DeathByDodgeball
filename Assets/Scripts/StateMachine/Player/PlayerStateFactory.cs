public class PlayerStateFactory
{
    PlayerStateMachine _context;

    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;  
    }

    public PlayerBaseState Active()
    {
        return new PlayerActiveState(_context, this);
    }
    public PlayerBaseState Aim()
    {
        return new PlayerAimState(_context, this);
    }
    public PlayerBaseState Throw()
    {
        return new PlayerThrowState(_context, this);
    }
    public PlayerBaseState Catch()
    {
        return new PlayerCatchState(_context, this);
    }
    public PlayerBaseState Dodge()
    {
        return new PlayerDodgeState(_context, this);
    }
    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Move()
    {
        return new PlayerMoveState(_context, this);
    }
    public PlayerBaseState Death()
    {
        return new PlayerDeathState(_context, this);
    }
}
