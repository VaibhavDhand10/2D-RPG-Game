using System;
using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill Data - ")]
public class Skill_DataSO : ScriptableObject
{
    public int cost;
    public SkillType skillType;
    public UpgradeData upgradeData;

    [Header("Skill Description")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
}