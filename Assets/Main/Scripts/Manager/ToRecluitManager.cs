using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToRecluitManager : MonoBehaviour
{
    private int MAX_TO_RECLUIT_ONSCREEN = 10;
    public int count = 0;
    public bool HasRoom()
    {
        return count < MAX_TO_RECLUIT_ONSCREEN;
    }
}
