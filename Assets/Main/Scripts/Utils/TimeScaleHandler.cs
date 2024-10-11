using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleHandler : MonoBehaviour
{
   
    public void TimeScaleOn()
    {
        Time.timeScale = 1;
    } 
    public void TimeScaleOff()
    {
        Time.timeScale = 0;
    }
}
