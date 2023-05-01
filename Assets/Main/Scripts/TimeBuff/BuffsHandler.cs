using System;
using UnityEngine;

public class BuffsHandler
{
    private Character character;
    private CustomList<AbstractTimeBuff> list = new CustomList<AbstractTimeBuff>();
    private float time = 0;
    public BuffsHandler(Character character)
    {
        this.character = character;
    }
    
    public void AddGenericBuff(Action onEnd, float time)
    {
        TimeBuffGeneric temp = new TimeBuffGeneric(character);
        temp.OnEnd = onEnd;
        temp.Init(time);
        int bestPosition = 0;
        foreach (var item in list)
        {
            if (item.GetTime() < time)
            {
                bestPosition++;
            }
            else
            {
                break;
            }
        }
        list.AddInPosition(temp, bestPosition);

    }
    public bool IsActive(Type t) 
    {
        foreach (var item in list)
        {
            if (t == item.GetType())
            {
                return true;
            }
        }
        return false;
    }
    public void InitBuff(Type t, float time)
    {     
        time += this.time;
        int bestPosition = 0;
   
        foreach (var item in list)
        {
            if (item.GetTime() < time)
            {
                bestPosition++;
            }
            if (t == item.GetType())
            {
                item.Init(time);
                break;
            }
        }  
    }
    public void ResetAll()
    {
        foreach (var item in list)
        {
            item.End();
        }
        list.Clear();
    }
    public void Update()
    {   
            foreach (var item in list)
            {
                item.Update(Time.deltaTime);
            }
            var current = list[0];
            while (current != null && current.IsReady())
            {
                current.End();
                list.Remove(current);
                current = list[0];
            }   
    }
    public void LateUpdate()
    {
        foreach (var item in list)
        {
            item.LateUpdate();
        }
    }
}
