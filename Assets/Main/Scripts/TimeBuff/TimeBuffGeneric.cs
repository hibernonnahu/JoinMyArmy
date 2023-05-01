using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeBuffGeneric : AbstractTimeBuff
{
    private Character character;
    private Action onEnd;
    public Action OnEnd
    {
        set
        {
            onEnd = value;
        }
    }
    public TimeBuffGeneric(Character character) : base()
    {
        this.character = character;
    }
    public override void Init(float initialTime)
    {
        base.Init(initialTime);
        
    }
    public override void End()
    {
        onEnd();
    }
}
