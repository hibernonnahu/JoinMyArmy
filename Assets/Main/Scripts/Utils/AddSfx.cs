using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSfx : MonoBehaviour
{
    public List<SFxManager.DictionaryFill> dictionaryFill;
    // Start is called before the first frame update
    void Start()
    {
        var sfxManager = FindObjectOfType<SFxManager>();
        if(sfxManager!=null)
        sfxManager.FillDictionary(dictionaryFill);
    }

    
}
