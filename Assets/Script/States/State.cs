using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    private string StateID;
    protected State(string _stateID) { StateID = _stateID; }

    public string GetStateID() { return StateID; }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
