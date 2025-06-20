using UnityEngine;
using UnityEngine.Rendering;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string animBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Entity_Stats stats;

    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        anim.SetBool(animBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParamater();
    }

    public virtual void Exit()
    {
        anim.SetBool(animBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    public virtual void UpdateAnimationParamater()
    {

    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stats.offense.attackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultiplier", attackSpeed);
    }
}