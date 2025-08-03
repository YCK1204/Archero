using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 스킬 데이터를 담는 순수 C# 클래스 (이제 이 파일 안에만 존재합니다)
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

    #region  등급별 버프 수치 클래스
    [System.Serializable] public class CommonBuffs { public float attackMultiplier = 1.2f; public float attackSpeedMultiplier = 1.2f; public float moveSpeedMultiplier = 1.2f; public float maxHealthMultiplier = 1.5f; [Range(0, 1)] public float healPercent = 0.2f; }
    [System.Serializable] public class RareBuffs { public float attackMultiplier = 1.3f; public float attackSpeedMultiplier = 1.3f; public float moveSpeedMultiplier = 1.3f; public float maxHealthMultiplier = 2.0f; [Range(0, 1)] public float healPercent = 0.3f; }
    [System.Serializable] public class EpicBuffs { public float attackMultiplier = 1.4f; public float attackSpeedMultiplier = 1.4f; public float moveSpeedMultiplier = 1.4f; public float maxHealthMultiplier = 2.5f; [Range(0, 1)] public float healPercent = 0.4f; }
    [System.Serializable] public class LegendaryBuffs { public float attackMultiplier = 1.5f; public float attackSpeedMultiplier = 1.5f; public float moveSpeedMultiplier = 1.5f; public int healthCost = 1; }
    #endregion

    [Header("버프 수치 라이브러리")]
    public CommonBuffs commonBuffs;
    public RareBuffs rareBuffs;
    public EpicBuffs epicBuffs;
    public LegendaryBuffs legendaryBuffs;

    private List<Skill> allSkills = new List<Skill>();
    private CharacterStats playerStats;

    void Awake()
    {
        Instance = this;
        
        // 버프 변수들이 null이면 기본값으로 초기화
        if (commonBuffs == null) commonBuffs = new CommonBuffs();
        if (rareBuffs == null) rareBuffs = new RareBuffs();
        if (epicBuffs == null) epicBuffs = new EpicBuffs();
        if (legendaryBuffs == null) legendaryBuffs = new LegendaryBuffs();
        
        BuildSkillDatabase();
    }

    void Start()
    {
        playerStats = FindObjectOfType<CharacterStats>();
    }

    private void BuildSkillDatabase()
        {
        // --- 레벨업 & 발키리 이벤트 스킬 (ID, 이름, 등급, 카테고리) ---
        allSkills.Add(new Skill("AttackUp", "공격력 증가", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackUp", "공격력 증가", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackUp", "공격력 증가", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackSpeedUp", "공격속도 증가", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackSpeedUp", "공격속도 증가", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("AttackSpeedUp", "공격속도 증가", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MoveSpeedUp", "이동속도 증가", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MoveSpeedUp", "이동속도 증가", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MoveSpeedUp", "이동속도 증가", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MaxHealthUp", "최대체력 증가", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MaxHealthUp", "최대체력 증가", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("MaxHealthUp", "최대체력 증가", ESkillGrade.Epic, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("Heal", "HP 회복", ESkillGrade.Common, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("Heal", "HP 회복", ESkillGrade.Rare, ESkillCategory.LevelUp));
        allSkills.Add(new Skill("Heal", "HP 회복", ESkillGrade.Epic, ESkillCategory.LevelUp));

        // --- 천사 이벤트 스킬 ---
        allSkills.Add(new Skill("Heal", "HP 회복", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("AttackUp", "공격력 증가", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("AttackSpeedUp", "공격속도 증가", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("MoveSpeedUp", "이동속도 증가", ESkillGrade.Common, ESkillCategory.Angel));
        allSkills.Add(new Skill("MaxHealthUp", "최대체력 증가", ESkillGrade.Common, ESkillCategory.Angel));

        // --- 악마 이벤트 스킬 ---
        allSkills.Add(new Skill("Devil_AttackUp", "악마의 거래: 공격력", ESkillGrade.Legendary, ESkillCategory.Devil));
        allSkills.Add(new Skill("Devil_AttackSpeedUp", "악마의 거래: 공속", ESkillGrade.Legendary, ESkillCategory.Devil));
        allSkills.Add(new Skill("Devil_MoveSpeedUp", "악마의 거래: 이속", ESkillGrade.Legendary, ESkillCategory.Devil));
    }

    #region 이벤트별 스킬 호출 함수
    public void ShowLevelUpSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.LevelUp).ToList());
    public void ShowValkyrieSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.LevelUp).ToList()); // 레벨업과 동일
    public void ShowAngelSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.Angel).ToList());
    public void ShowDevilSkills() => ShowSkillSelectionUI(allSkills.Where(s => s.Category == ESkillCategory.Devil).ToList());
    #endregion

    private void ShowSkillSelectionUI(List<Skill> skillsToShow) {/* UI 로직 */}

    //  리스트 접근 함수 추가
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
        // playerStats가 null이면 다시 찾기
        if (playerStats == null)
        {
            playerStats = FindObjectOfType<CharacterStats>();
            if (playerStats == null)
            {
                Debug.LogWarning("CharacterStats를 찾을 수 없습니다. 스킬 효과를 적용할 수 없습니다.");
                return;
            }
        }

        switch (skill.EffectID)
        {
            // --- 일반 버프 ---
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

            // --- 악마의 계약 ---
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

     // 등급과 스탯 종류에 따라 올바른 배율을 찾아주는 보조 함수
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
        return 1f; // 기본값
    }


}