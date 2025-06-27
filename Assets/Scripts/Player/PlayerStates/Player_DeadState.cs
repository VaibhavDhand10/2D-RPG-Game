using Unity.VisualScripting;
using UnityEngine;

public class Player_DeadState : PlayerState
{
    public Player_DeadState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        input.Disable();
        rb.simulated = false;
    }

    public override void Update()
    {
        base.Update();

    }
}