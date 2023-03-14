
using UnityEngine;

public class CharacterMain : Character
{
    [Header("Main ExternalElements")]
    public FloatingJoystick floatingJoystick;

    private void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        stateMachine.AddState(new StateCharacterMainInGame(stateMachine, this));
    }
    protected override void Update()
    {
        stateMachine.CurrentState.UpdateMovement(floatingJoystick.Horizontal, floatingJoystick.Vertical);
        base.Update();//updates stateMachine
    }


}
