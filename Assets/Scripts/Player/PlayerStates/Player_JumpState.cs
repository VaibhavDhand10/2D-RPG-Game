using UnityEngine;

public class Player_JumpState : Player_AiredState
{
    public Player_JumpState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(rb.linearVelocity.x, player.jumpForce);
    }

    public override void Update()
    {
        base.Update();

        //we need to be sure we are not in jumpAttackState when we enter fallState because then player never goes to jumpAttackState
        if (rb.linearVelocity.y < 0 && stateMachine.currentState != player.jumpAttackState)
            stateMachine.ChangeState(player.fallState);
    }
}