using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base (stateMachine, animBoolName)
    {
        this.player = player;

        rb = player.rb;
        anim = player.anim;
        stats = player.stats;
        input = player.input;
    }

    public override void Update()
    {
        base.Update();


        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
            stateMachine.ChangeState(player.dashState);
    }

    public override void UpdateAnimationParamater()
    {
        base.UpdateAnimationParamater();
        anim.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private bool CanDash()
    {
        if (player.wallDetected)
            return false;
        if (stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}