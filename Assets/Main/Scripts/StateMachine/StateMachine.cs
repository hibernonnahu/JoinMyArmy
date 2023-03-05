using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class StateMachine<S> where S:State<S>
{
	private Dictionary<String,S> stateDictionary;
	private S currentState;
    public S CurrentState
    {
        get
        {
           return currentState;
        }
    }
	private Type lastState;
    public Type GetLastState()
    {
        return lastState;
    }

	public StateMachine ()
	{
			stateDictionary = new Dictionary<String, S> ();
	}

	public void AddState (S state,String customName="")
	{
        if(customName=="")
        {
            customName = state.GetType().Name;
        }
			if (state != null) {
					stateDictionary.Add (customName,state);
					if (stateDictionary.Count == 1) {
							currentState = state;
							state.Awake ();
					}
			}
	}

    public S ChangeState(String type)
    {
        S aux = null;
        stateDictionary.TryGetValue(type, out aux);
        if (aux != null && !aux.Equals(currentState))
        {
            currentState.Sleep();
            lastState = currentState.GetType();

            currentState = aux;
            currentState.Awake();
        }

        return currentState;
    }
    public S ChangeState (Type type)
	{
        return ChangeState(type.Name);
	}
    public void ChangeState<T>() where T : S
    {
        ChangeState(typeof(T));
    }


    public void Update ()
		{
				currentState.Update ();

		}
    public void FixedUpdate()
    {
        currentState.FixedUpdate();
    }
    public T GetState<T>() where T : S
    {
        S aux = null;
        stateDictionary.TryGetValue(typeof(T).Name, out aux);
        return (T)aux;
    }

}
