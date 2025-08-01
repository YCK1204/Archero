using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// ��ų �����͸� ��� ���� C# Ŭ���� (���� �� ���� �ȿ��� �����մϴ�)
public class Skill
{
    public string EffectID { get; }
    public string Name { get; }
    public ESkillGrade Grade { get; }
    public ESkillCategory Category { get; }

    public Skill(string id, string name, ESkillGrade grade, ESkillCategory category)
    {
        EffectID = id; Name = name; Grade = grade; Category = category;
    }
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    #region ��޺� ���� ��ġ Ŭ����
    [System.Serializable] public class CommonBuffs { public float attackMultiplier = 1.2f; public float attackSpeedMultiplier = 1.2f; public float moveSpeedMultiplier = 1.2f; public float maxHealthMultiplier = 1.5f; [Range(0, 1)] public float healPercent = 0.2f; }
    [System.Serializable] public class RareBuffs { public float attackMultiplier = 1.3f; public float attackSpeedMultiplier = 1.3f; public float moveSpeedMultiplier = 1.3f; public float maxHealthMultiplier = 2.0f; [Range(0, 1)] public float healPercent = 0.3f; }
    [System.Serializable] public class EpicBuffs { public float attackMultiplier = 1.4f; public float attackSpeedMultiplier = 1.4f; public float moveSpeedMultiplier = 1.4f; public float maxHealthMultiplier = 2.5f; [Range(0, 1)] public float healPercent = 0.4f; }
    [System.Serializable] public class LegendaryBuffs { public float attackMultiplier = 1.5f; public float attackSpeedMultiplier = 1.5f; public float moveSpeedMultiplier = 1.5f; public int healthCost = 1; }
    #endregion

    [Header("���� ��ġ ���̺귯��")]
    public CommonBuffs commonBuffs;
    public RareBuffs rareBuffs;
    public EpicBuffs epicBuffs;
    public LegendaryBuffs legendaryBuffs;

    private List<Skill> allSkills = new List<Skill>();
    private CharacterStats playerStats;

    void Awake()
    {
        Instance = this;
        BuildSkillDatabase();
    }

    void Start()
    {
        playerStats = FindObjectOfType<CharacterStats>();
    }

    private void BuildSkillDatabase()
    {
        // --- ������ & ��Ű�� �̺�Ʈ ��ų (ID, �̸�, ���, ī�װ�) ---
        allSkills.Add(new Skill("AttackUp", "���ݷ� ����", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackUp", "���ݷ� ����", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackUp", "���ݷ� ����", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackSpeedUp", "���ݼӵ� ����", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackSpeedUp", "���ݼӵ� ����", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackSpeedUp", "���ݼӵ� ����", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MoveSpeedUp", "�̵��ӵ� ����", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MoveSpeedUp", "�̵��ӵ� ����", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MoveSpeedUp", "�̵��ӵ� ����", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MaxHealthUp", "�ִ�ü�� ����", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MaxHealthUp", "�ִ�ü�� ����", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MaxHealthUp", "�ִ�ü�� ����", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("Heal", "HP ȸ��", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("Heal", "HP ȸ��", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("Heal", "HP ȸ��", ESkillGrade.Epic, ESkillCategory.LevelUp));

        // --- õ�� �̺�Ʈ ��ų ---
        allSkills.Add(new Skill("Heal", "HP ȸ��", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("AttackUp", "���ݷ� ����", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("AttackSpeedUp", "���ݼӵ� ����", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("MoveSpeedUp", "�̵��ӵ� ����", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("MaxHealthUp", "�ִ�ü�� ����", ESkillGrade.Common, ESkillCategory.Angel));

        // --- �Ǹ� �̺�Ʈ ��ų ---
        allSkills.Add(new Skill("Devil_AttackUp", "�Ǹ��� �ŷ�: ���ݷ�", ESkillGrade.Legendary, ESkillCategory.Devil));
        allSkills.Add(new Skill("Devil_AttackSpeedUp", "�Ǹ��� �ŷ�: ����", ESkillGrade.Legendary, ESkillCategory.Devil));
        allSkills.Add(new Skill("Devil_MoveSpeedUp", "�Ǹ��� �ŷ�: �̼�", ESkillGrade.Legendary, ESkillCategory.Devil));
    }

    #region �̺�Ʈ�� ��ų ȣ�� �Լ�
    public void ShowLevelUpSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.LevelUp).ToList());
    public void ShowValkyrieSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.LevelUp).ToList()); // �������� ����
    public void ShowAngelSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.Angel).ToList());
    public void ShowDevilSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.Devil).ToList());
    #endregion

    private void ShowSkillSelectionUI(List<Skill> skillsToShow) { /* UI ���� */ }

    //  ����Ʈ ���� �Լ� �߰�
    public List<Skill> GetAllSkills()
    {
        return allSkills;
    }
    public Skill GetSkillInfo(string effectID, ESkillGrade grade, ESkillCategory category)
    {
        return allSkills.FirstOrDefault(skill => skill.EffectID == effectID &&skill.Grade == grade && skill.Category == category);
    }

    // UI���� ��ų�� �����ϸ� �� �Լ��� ȣ��
    public void SelectSkill(Skill selectedSkill)
    {
        ApplySkillEffect(selectedSkill);
    }

    private void ApplySkillEffect(Skill skill)
    {
        switch (skill.EffectID)
        {
            // --- ���� ���� ---
            case "AttackUp":
                playerStats.MultiplyStat(EStatType.Attack, GetStatMultiplier(skill.Grade, EStatType.Attack));
                break;
            case "AttackSpeedUp":
                playerStats.MultiplyStat(EStatType.AttackSpeed, GetStatMultiplier(skill.Grade, EStatType.AttackSpeed));
                break;
            case "MoveSpeedUp":
                playerStats.MultiplyStat(EStatType.MoveSpeed, GetStatMultiplier(skill.Grade, EStatType.MoveSpeed));
                break;
            case "MaxHealthUp":
                playerStats.MultiplyMaxHealth(GetStatMultiplier(skill.Grade, EStatType.MaxHealth));
                break;
            case "Heal":
                playerStats.HealByPercentage(GetStatMultiplier(skill.Grade, EStatType.Heal));
                break;

            // --- �Ǹ��� �ŷ� ---
            case "Devil_AttackUp":
                playerStats.MultiplyStat(EStatType.Attack, legendaryBuffs.attackMultiplier);
                playerStats.DecreaseMaxHealth(legendaryBuffs.healthCost);
                break;
            case "Devil_AttackSpeedUp":
                playerStats.MultiplyStat(EStatType.AttackSpeed, legendaryBuffs.attackSpeedMultiplier);
                playerStats.DecreaseMaxHealth(legendaryBuffs.healthCost);
                break;
            case "Devil_MoveSpeedUp":
                playerStats.MultiplyStat(EStatType.MoveSpeed, legendaryBuffs.moveSpeedMultiplier);
                playerStats.DecreaseMaxHealth(legendaryBuffs.healthCost);
                break;
        }
    }

    // ��ް� ���� ������ ���� �ùٸ� ������ ã���ִ� ���� �Լ�
    private float GetStatMultiplier(ESkillGrade grade, EStatType type)
    {
        switch (grade)
        {
            case ESkillGrade.Common:
                if (type == EStatType.Attack) return commonBuffs.attackMultiplier;
                if (type == EStatType.AttackSpeed) return commonBuffs.attackSpeedMultiplier;
                if (type == EStatType.MoveSpeed) return commonBuffs.moveSpeedMultiplier;
                if (type == EStatType.MaxHealth) return commonBuffs.maxHealthMultiplier;
                if (type == EStatType.Heal) return commonBuffs.healPercent;
                break;
            case ESkillGrade.Rare:
                if (type == EStatType.Attack) return rareBuffs.attackMultiplier;
                if (type == EStatType.AttackSpeed) return rareBuffs.attackSpeedMultiplier;
                if (type == EStatType.MoveSpeed) return rareBuffs.moveSpeedMultiplier;
                if (type == EStatType.MaxHealth) return rareBuffs.maxHealthMultiplier;
                if (type == EStatType.Heal) return rareBuffs.healPercent;
                break;
            case ESkillGrade.Epic:
                if (type == EStatType.Attack) return epicBuffs.attackMultiplier;
                if (type == EStatType.Heal) return epicBuffs.healPercent;
                if (type == EStatType.MoveSpeed) return epicBuffs.moveSpeedMultiplier;
                if (type == EStatType.MaxHealth) return epicBuffs.maxHealthMultiplier;
                if (type == EStatType.Heal) return epicBuffs.healPercent;
                break;
            case ESkillGrade.Legendary:
                if (type == EStatType.Attack) return legendaryBuffs.attackMultiplier;
                if (type == EStatType.Heal) return legendaryBuffs.attackSpeedMultiplier;
                if (type == EStatType.MoveSpeed) return legendaryBuffs.moveSpeedMultiplier;
                break;
        }
        return 1f; // �⺻��
    }


}