using UnityEngine;
using System.Collections;
using System;

public class State<T> where T:State<T>
{
    protected StateMachine<T> stateMachine;
   
    // Use this for initialization
    public State(StateMachine<T> sm)
    {
        stateMachine = sm;
       
    }

    public virtual void Awake()
    {

    }

    public virtual void Sleep()
    {

    }

    public virtual void Update()
    {

    }
    public virtual void FixedUpdate() { }
    public virtual void ChangeState(Type type)
    {
        this.stateMachine.ChangeState(type);
    }
}
