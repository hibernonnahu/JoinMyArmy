using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalitycsPrefab : MonoBehaviour
{
    public Text keyName;
    public Text resultText;
    public float result = 0;
    private Analitycs analitycs;
    private string key;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Init(Analitycs analitycs,string key,bool metric=true)
    {
        this.analitycs = analitycs;
        this.key =key;
        keyName.text = key;
        if (metric)
        {
            ParseMetricValues();
            keyName.text = "m_"+key;
        }
        else
        {
            GetComponent<Image>().color = Color.cyan;
            ParseIntValues();
            keyName.text = "o_" + key;
        }
        
    }
    private void ParseMetricValues()
    {
        List<int> values = new List<int>();
        foreach (var data in analitycs.Savedata)
        {
            string value = data.GetMetric(key, "");
            if (value != "")
            {
                int r = -1;
                int.TryParse(value, out r);
                if (r != -1)
                {
                    values.Add(r);
                }
            }
           
        }
        result = CustomMath.Average(values);

        resultText.text = result.ToString("f2");
    }
    private void ParseIntValues()
    {
        List<int> values = new List<int>();
        foreach (var data in analitycs.Savedata)
        {
            int value = data.GetValue(key, -1);
            if (value != -1)
            {
               
                    values.Add(value);
                
            }

        }
        result = CustomMath.Average(values);
        resultText.text = result.ToString("f2");
    }

    public void CopyKey()
    {
        GUIUtility.systemCopyBuffer = keyName.text;
    }
}
