using System.Diagnostics;

public abstract class CPUBaseState
{
    private bool _isRootState = false;
    private CPUBrain _ctx;
    private CPUStateFactory _factory;
    private CPUBaseState _currentSuperState;
    private CPUBaseState _currentSubState;

    protected bool IsRootState { set { _isRootState = value; } }
    protected CPUBrain Ctx { get { return _ctx; } }
    protected CPUStateFactory Factory { get { return _factory; } }
    private CPUBaseState CurrentSuperState { get { return _currentSuperState; } }
    private CPUBaseState CurrentSubState { get { return CurrentSubState; } }


    public CPUBaseState(CPUBrain ctx, CPUStateFactory factory)
    {
        _ctx = ctx;
        _factory = factory;
    }

    public abstract void EnterState();

    public abstract void UpdateState();

    public abstract void FixedUpdateState();

    public abstract void ExitState();

    public abstract void InitializeSubState();

    public abstract void CheckSwitchState();

    public void UpdateStates()
    {
        UpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.UpdateStates();
        }
    }

    public void FixedUpdateStates()
    {
        FixedUpdateState();
        if (_currentSubState != null)
        {
            _currentSubState.FixedUpdateStates();
        }
    }
    protected void SwitchState(CPUBaseState newState)
    {
        ExitState();
        newState.EnterState();

        if (_isRootState)
        {
            _ctx.CurrentState = newState;
        }
        else if (_currentSuperState != null)
        {
            _currentSuperState.SetSubState(newState);
        }
    }

    protected void SetSuperState(CPUBaseState newSuperState)
    {
        _currentSuperState = newSuperState;
    }

    protected void SetSubState(CPUBaseState newSubState)
    {
        _currentSubState = newSubState;
        newSubState.SetSuperState(this);
    }
}
