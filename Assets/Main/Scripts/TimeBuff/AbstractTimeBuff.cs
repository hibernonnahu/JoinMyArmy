using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AbstractTimeBuff
{
    protected float time;
    private float initialTime;
   
    public AbstractTimeBuff() { }
    public virtual void Init(float initialTime)
    {
        this.time = this.initialTime = initialTime;
        
    }
    public virtual void Update(float tick)
    {
        time -= tick;
    }
    public float GetTime()
    {
        return time;
    }
   
    public virtual bool IsReady()
    {
       // Debug.Log(this + " " + time);
        return time <= 0;
    }
    public virtual void End() {
    }
    public virtual void LateUpdate() { }
}
