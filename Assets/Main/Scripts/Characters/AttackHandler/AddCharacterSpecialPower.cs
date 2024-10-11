using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCharacterSpecialPower : MonoBehaviour, ICharacterSpecialPower
{
   
    public IconUIController iconUIController;
    public float uIColdDown = 5;
    public int manaCost = 50;
    private Action onUpdate = () => { };
    private float counter;
    protected CharacterMain character;
    public virtual float Execute()
    {

        counter = uIColdDown;

        onUpdate = () =>
        {
            counter -= Time.deltaTime;
            if (counter < 0)
            {

                onUpdate = () => { };
            }
        };
        character.mana -= manaCost;
        ExecutePower();
        return uIColdDown;


    }

    public virtual AddCharacterSpecialPower Init(CharacterMain character)
    {
        this.character = character;//also states
        iconUIController.Character = character;
       
        iconUIController.AddExtraRequirement(IsManaEnough);
        character.SetCastMainPower(this);
        return this;
    }
    private bool IsManaEnough()
    {
        return character.mana >= manaCost;
    }

    private void Update()
    {
        onUpdate();
    }

    protected virtual void ExecutePower()
    {

    }
}
