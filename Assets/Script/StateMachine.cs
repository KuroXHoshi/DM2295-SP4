using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    Dictionary<string, State> StateMap;
    State CurrState = null, NextState = null;

    public void AddState(State newState)
    {
        if (newState != null || StateMap.ContainsKey(newState.GetStateID()))
            return;

        if (CurrState != null)
            CurrState = NextState = newState;

        StateMap.Add(newState.GetStateID(), newState);
    }

    public void SetNextState(string nextStateID)
    {
        if (CurrState.GetStateID() == nextStateID)
            return;

        if (!StateMap.TryGetValue(nextStateID, out NextState))
            Debug.Log(this.GetType() + ".cs : Unable to SetNextState!");
    }

    public string GetCurrentState()
    {
        if (CurrState != null)
            return CurrState.GetStateID();
        return "<No States>";
    }

    // Update is called once per frame
    public void Update ()
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
