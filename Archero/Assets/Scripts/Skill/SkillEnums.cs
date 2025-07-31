// 이 파일은 오직 Enum(타입) 정의만을 위해 존재

/// <summary>
/// 스킬이 등장하는 이벤트의 종류
/// </summary>
public enum ESkillCategory
{
    LevelUp,
    Valkyrie,
    Angel,
    Devil
}

/// <summary>
/// 스킬의 등급
/// </summary>
public enum ESkillGrade
{
    Common,
    Rare,
    Epic,
    Legendary
}

/// <summary>
/// 스탯 강화의 종류
/// </summary>
public enum EStatType
{
    Attack,
    AttackSpeed,
    MoveSpeed,
    MaxHealth,
    Heal,
    PetAttack // 펫 공격력
}