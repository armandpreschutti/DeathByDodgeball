using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUBrain : MonoBehaviour
{
    [Header("States")]
    [SerializeField] CPUBaseState _currentState;
    [SerializeField] CPUStateFactory _states;
    public string CurrentSuperState;
    public string CurrentSubState;

    public CPUBaseState CurrentState { get { return _currentState; } set { _currentState = value; } }


    private void Awake()
    {
        _states = new CPUStateFactory(this);
        _currentState = _states.Active();
        _currentState.EnterState();

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
    }

    private void FixedUpdate()
    {
        _currentState.FixedUpdateStates();
    }
}
