using QUDOSDK;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QudoState : MonoBehaviour
{
    public bool idle = true;
    // Start is called before the first frame update
    void Start()
    {
        if (idle) {
            QUDO.CurrentGameState = GameState.IDLE;
                }
        else
        {
            QUDO.CurrentGameState = GameState.PLAYING;

        }
    }

    
}
