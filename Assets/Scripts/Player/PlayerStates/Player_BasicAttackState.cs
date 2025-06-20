using System.Linq;
using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private float lastTimeAttack;

    private bool comboAttackQueued;
    private int comboIndex = 1;
    private int comboLimit = 3;
    private int attackDir;
    private const int FirstComboIndex = 1; // We start combo index with index 1, this is used in the Animator

    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
            comboLimit = player.attackVelocity.Length; // safety check for comboLimit, it should be the same as the attackVelocity array length
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndexIfNeeded();
        SyncAttackSpeed();

        // Define attack direction according to input
        if (player.moveInput.x != 0)
            attackDir = ((int)player.moveInput.x);
        else
            attackDir = player.facingDir;
        // attackDir = player.moveInput.x != 0 ? ((int)player.moveInput.x) : player.facingDir; alternate code

        anim.SetInteger("BasicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
            QueueNextAttack();

        if (triggerCalled)
            HandleStateExit();
        
    }

    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(animBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
            stateMachine.ChangeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttack = Time.time;
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
            comboAttackQueued = true;
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
            player.SetVelocity(0, rb.linearVelocity.y);
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];

        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDir, attackVelocity.y);
    }
    
    private void ResetComboIndexIfNeeded()
    {
        if (Time.time > lastTimeAttack + player.comboResetTime)
            comboIndex = FirstComboIndex;

        if (comboIndex > comboLimit)
            comboIndex = FirstComboIndex;
    }
}