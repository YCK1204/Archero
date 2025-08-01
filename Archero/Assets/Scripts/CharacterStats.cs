using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private Stat baseStats;
    public Stat TotalStats { get; private set; }

    [Header("캐릭터 정보")]
    public int Level = 1;
    // 아이작 스타일: CurrentHealth는 하트 반 칸의 개수를 의미 (최대 체력 = MaxHeartContainers * 2)
    public int CurrentHealth;
    public int CurrentExp { get; private set; }
    public int MaxExp { get; private set; } = 100; // 예시: 레벨업에 필요한 경험치

    void Awake()
    {
        // Stat(공격력, 최대하트, 공속, 이속, 대쉬속도)
        baseStats = new Stat(10, 3, 1f, 5f, 15f); // 기본 스탯: 공격력 10, 하트 3칸, 공속 1
        TotalStats = baseStats;
        // 현재 체력을 최대 하트 칸의 2배로 설정 (하트 한 칸 = 체력 2)
        CurrentHealth = TotalStats.MaxHeartContainers * 2;
    }

    // 스탯을 곱연산으로 강화하는 함수
    public void MultiplyStat(EStatType statType, float multiplier)
    {
        var newStats = TotalStats; // 값 형식인 struct를 수정하기 위해 복사본을 만듭니다.
        switch (statType)
        {
            case EStatType.Attack:
                newStats.Attack = (int)(newStats.Attack * multiplier);
                break;
            case EStatType.AttackSpeed:
                newStats.AttackSpeed *= multiplier;
                break;
            case EStatType.MoveSpeed:
                newStats.MoveSpeed *= multiplier;
                break;
        }
        TotalStats = newStats; // 변경된 복사본을 다시 원본에 할당합니다.
        Debug.Log($"{statType} 스탯이 {multiplier}배 만큼 적용! 현재 공격력: {TotalStats.Attack}");
    }
    // 최대 체력을 곱연산으로 강화하는 함수
    public void MultiplyMaxHealth(float multiplier)
    {
        var newStats = TotalStats;
        newStats.MaxHeartContainers = (int)(newStats.MaxHeartContainers * multiplier);
        TotalStats = newStats;
        Heal(TotalStats.MaxHeartContainers * 2); // 체력도 최대치에 맞게 보정
    }
    // 최대 체력을 고정 수치만큼 감소시키는 함수 (악마 거래용)
    public void DecreaseMaxHealth(int amount)
    {
        var newStats = TotalStats;
        newStats.MaxHeartContainers -= amount;
        if (newStats.MaxHeartContainers < 1) newStats.MaxHeartContainers = 1; // 최소 1칸은 유지
        TotalStats = newStats;

        if (CurrentHealth > TotalStats.MaxHeartContainers * 2)
        {
            CurrentHealth = TotalStats.MaxHeartContainers * 2;
        }
    }
    /// <summary>
    /// 데미지를 받는 함수 (체력 1칸 감소)
    /// </summary>
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log($"피해 {damage}! 남은 체력: {CurrentHealth}");
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    // 최대 체력 대비 %로 회복하는 함수
    public void HealByPercentage(float percent)
    {
        int healAmount = (int)(TotalStats.MaxHeartContainers * 2 * percent);
        Heal(healAmount);
    }
    /// <summary>
    /// 체력을 회복하는 함수 (하트 반 칸 단위)
    /// </summary>
    public void Heal(int amount)
    {
        CurrentHealth += amount;
        if (CurrentHealth > TotalStats.MaxHeartContainers * 2)
        {
            CurrentHealth = TotalStats.MaxHeartContainers * 2;
        }
    }

    /// <summary>
    /// 경험치를 획득하는 함수
    /// </summary>
    public void AddExperience(int amount)
    {
        CurrentExp += amount;
        Debug.Log($"경험치 {amount} 획득! 현재 경험치: {CurrentExp}/{MaxExp}");
        if (CurrentExp >= MaxExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        Level++;
        CurrentExp -= MaxExp; // 남은 경험치 이월
        MaxExp += 50; // ex) 다음 레벨업에 필요한 경험치 증가

        // 레벨업 시 스탯 보너스 적용 로직 추가
        Stat levelUpBonus = new Stat(0.5f, 1, 0.5f, 0f, 0f);

        // TODO: SkillManager를 호출하여 스킬 선택 창을 띄웁니다.
        // SkillManager.Instance.ShowSkillSelectionUI();
    }

    // 사망 관련 함수
    private void Die()
    {
        // 사망 로직 추가
    }
}