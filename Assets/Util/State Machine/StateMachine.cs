using UnityEngine;

public class StateMachine<T>
{
    public State<T> Currentstate { get; private set; }

    T _owner;
    public StateMachine(T owner)
    {
        _owner = owner;
    }

    public void ChangeState(State<T> newState)
    {
        Currentstate?.Exit();
        Currentstate = newState;
        Currentstate.Enter(_owner);
    }

    public void Execute()
    {
        Currentstate?.Execute();

    }
}
