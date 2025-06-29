using UnityEngine;

public class Skill_Dash : Skill_Base
{
    public void OnStartEffect()
    {
        if(Unlocked(SkillUpgradeType.Dash_CloneOnStart) || Unlocked(SkillUpgradeType.Dash_CloneOnStartandArrival))
            CreateClone();

        if(Unlocked(SkillUpgradeType.Dash_ShardOnStart) || Unlocked(SkillUpgradeType.Dash_ShardOnStartandArrival))
            CreateShard();
    }

    public void OnEndEffect()
    {
        if(Unlocked(SkillUpgradeType.Dash_CloneOnStartandArrival))
            CreateClone(); 

        if(Unlocked(SkillUpgradeType.Dash_ShardOnStartandArrival))
            CreateShard();
    }
    
    private void CreateShard()
    {
        Debug.Log("Create Time Shard");
    }

    private void CreateClone()
    {
        Debug.Log("Create Time Echo");
    }
}