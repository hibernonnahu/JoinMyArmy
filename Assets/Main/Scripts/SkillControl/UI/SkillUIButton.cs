using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUIButton : MonoBehaviour
{
    public Image image;
    public Text text;
    public string skillClassName;
    ISkill skill;
    private void Start()
    {
        var type = Type.GetType(skillClassName);
        if(type == null)
        {
            throw new Exception("this is not a class " + skillClassName);
        }
        else if(type.GetInterface("ISkill")==null)
        {
            throw new Exception("this is not an skill " + skillClassName);
        }
        skill= (ISkill)Activator.CreateInstance(type);
        text.text = skill.GetName();
        image.sprite = Resources.Load<Sprite>("Skills/"+skill.GetName());
    }
    public void OnClick()
    {
        FindObjectOfType<CharacterMain>().SkillController.AddSkill(skill);
        GetComponentInParent<Popup>().Close();
    }
}
