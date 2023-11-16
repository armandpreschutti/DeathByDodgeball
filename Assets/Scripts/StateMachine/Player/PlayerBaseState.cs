using System.Diagnostics;

public abstract class PlayerBaseState
{
    private bool _isRootState = false;
    private PlayerStateMachine _ctx;
    private PlayerStateFactory _factory;
    private PlayerBaseState _currentSuperState;
    private PlayerBaseState _currentSubState;

    protected bool IsRootState { set {  _isRootState = value; } }
    protected PlayerStateMachine Ctx { get { return _ctx; } }
    protected PlayerStateFactory Factory { get { return _factory; } }
    private PlayerBaseState CurrentSuperState { get { return _currentSuperState; } }
    private PlayerBaseState CurrentSubState { get { return CurrentSubState; } }


    public PlayerBaseState(PlayerStateMachine ctx, PlayerStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }
 
    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void FixedUpdateState();
    
    public abstract void ExitState();

    public abstract void InitializeSubState();

    public abstract void CheckSwitchState ();

    public void UpdateStates() 
    {
        UpdateState();
        if(_currentSubState != null)  
        {
            _currentSubState.UpdateStates();
        }
    }

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if(_currentSubState != null)
        {
            _currentSubState.FixedUpdateStates();
        }
    }
    protected void SwitchState(PlayerBaseState newState) 
    {
        ExitState();
        newState.EnterState();

        if(_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if(_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(PlayerBaseState newSuperState)
    {
        _currentSuperState = newSuperState; 
    }
    
    protected void SetSubState(PlayerBaseState newSubState)
    {
        _currentSubState = newSubState; 
        newSubState.SetSuperState(this);
    }
}
