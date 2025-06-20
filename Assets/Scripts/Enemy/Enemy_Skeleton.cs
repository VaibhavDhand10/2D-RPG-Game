using UnityEngine;

public class Enemy_Skeleton : Enemy , ICounterable
{
    public bool CanBeCountered { get => canBeStunned; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "Idle");
        moveState = new Enemy_MoveState(this, stateMachine, "Move");
        attackState = new Enemy_AttackState(this, stateMachine, "Attack");
        battleState = new Enemy_BattleState(this, stateMachine, "Battle");
        deadState = new Enemy_DeadState(this, stateMachine, "Idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "Stunned");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
    }

    public void HandleCounter()
    {
        if (!CanBeCountered) // if cannot be countered, dont be stunned
            return;
            
        stateMachine.ChangeState(stunnedState);
    }
}