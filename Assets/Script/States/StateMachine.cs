using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    Dictionary<string, State> StateMap = new Dictionary<string, State>();
    State CurrState = null, NextState = null;

    public bool HasStates()
    {
        if (StateMap == null)
            return false;

        return (StateMap.Count == 0) ? false : true;
    }

    public void AddState(State newState)
    {
        if (newState == null)
            return;

        if (CurrState == null)
            CurrState = NextState = newState;

        StateMap.Add(newState.GetStateID(), newState);
    }

    public void SetNextState(string nextStateID)
    {
        if (CurrState.GetStateID() == nextStateID)
            return;

        if (!StateMap.TryGetValue(nextStateID, out NextState))
        {
            NextState = CurrState;
            Debug.Log(this.GetType() + ".cs : Unable to SetNextState!");
        }
    }

    public string GetCurrentState()
    {
        if (CurrState != null)
            return CurrState.GetStateID();
        return "<No States>";
    }

    public bool IsCurrentState(string state)
    {
        if (CurrState == null || state == null)
            return false;

        return (CurrState.GetStateID() == state) ? true : false;
    }

    // Update is called once per frame
    public void Update (float deltaTime)
    {
		if (CurrState != NextState)
        {
            CurrState.Exit();
            CurrState = NextState;
            CurrState.Enter();
        }

        CurrState.Update();
	}
}
