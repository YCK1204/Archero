using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private Stat baseStats;
    public Stat TotalStats { get; private set; }

    [Header("캐릭터 정보")]
    // 아이작이나 건전처럼 CurrentHealth 1 당 하트 반 칸 (최대 체력 = MaxHeartContainers * 2)
    [SerializeField] private int currentHealth;
    public int CurrentHealth => currentHealth;
    public int Level => GetComponent<PlayerExpHandler>().CurrentPlayerLevel;
    void Awake()
    {
        // Stat(공격력, 최대하트, 공속, 이속, 대쉬속도)
        baseStats = new Stat(10, 3, 1f, 5f, 15f); // 기본 스탯: 공격력 10, 하트 3칸, 공속 1
        TotalStats = baseStats;
        // 현재 체력을 최대 하트 칸의 2배로 설정 (하트 한 칸 = 체력 2)
        currentHealth = TotalStats.MaxHeartContainers * 2;
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
    // 사망 관련 함수
    private void Die()
    {
        // 사망 로직 추가
    }
}