// �� ������ ���� Enum(Ÿ��) ���Ǹ��� ���� ����

/// <summary>
/// ��ų�� �����ϴ� �̺�Ʈ�� ����
/// </summary>
public enum ESkillCategory
{
    LevelUp,
    Valkyrie,
    Angel,
    Devil
}

/// <summary>
/// ��ų�� ���
/// </summary>
public enum ESkillGrade
{
    Common,
    Rare,
    Epic,
    Legendary
}

/// <summary>
/// ���� ��ȭ�� ����
/// </summary>
public enum EStatType
{
    Attack,
    AttackSpeed,
    MoveSpeed,
    MaxHealth,
    Heal,
    PetAttack // �� ���ݷ�
}

public enum EProjectileModifier
{
    FrontShot,      // ���� �߰� �߻�
    DiagonalShot,   // �缱 �߰� �߻�
    BackShot,       // �Ĺ� �߻�
    Piercing        // ����
}
