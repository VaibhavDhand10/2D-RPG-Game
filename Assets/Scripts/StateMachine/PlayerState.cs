using Unity.VisualScripting;
using UnityEngine;

public abstract class PlayerState : EntityState
{
    protected Player player;
    protected PlayerInputSet input;
    protected Player_SkillManager skillManager;

    public PlayerState(Player player, StateMachine stateMachine, string animBoolName) : base (stateMachine, animBoolName)
    {
        this.player = player;

        rb = player.rb;
        anim = player.anim;
        stats = player.stats;
        input = player.input;
        skillManager = player.skillManager;
    }

    public override void Update()
    {
        base.Update();


        if (input.Player.Dash.WasPressedThisFrame() && CanDash())
        {
            skillManager.dash.SetSkillOnCooldown();
            stateMachine.ChangeState(player.dashState);
        }
    }

    public override void UpdateAnimationParamater()
    {
        base.UpdateAnimationParamater();
        anim.SetFloat("yVelocity", rb.linearVelocityY);
    }

    private bool CanDash()
    {
        if(!skillManager.dash.canUseSkill())
            return false;
        
        if (player.wallDetected)
            return false;
        if (stateMachine.currentState == player.dashState)
            return false;

        return true;
    }
}