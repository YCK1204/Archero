using Assets.Define;
using Lee.Scripts;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("기본 능력치")]
    [SerializeField] private Stat baseStats = new Stat(10, 6, 1f, 5f, 15f);  // 인스펙터에서 조정 가능

    public Stat TotalStats { get; private set; }
    public int CurrentHp => currentHp;
    public int Level => GetComponent<PlayerExpHandler>().CurrentPlayerLevel;
    public event Action OnStatChanged;

    [SerializeField] private int currentHp;

    void Awake()
    {
        TotalStats = baseStats;
        currentHp = TotalStats.MaxHp;

        BattleManager.GetInstance.RegistHitInfo(GetComponent<Collider2D>(),TakeDamage);

        OnStatChanged?.Invoke();
    }

    // 스탯을 곱연산으로 강화하는 함수
    public void MultiplyStat(EStatType statType, float multiplier)
    {
        var newStats = TotalStats; // 값 형식인 struct를 수정하기 위해 복사본을 만듭니다.
        switch (statType)
        {
            case EStatType.Attack:
                newStats.AttackPower = (int)(newStats.AttackPower * multiplier);
                break;
            case EStatType.AttackSpeed:
                newStats.AttackSpeed *= multiplier;
                break;
            case EStatType.MoveSpeed:
                newStats.MoveSpeed *= multiplier;
                break;
        }
        TotalStats = newStats; // 변경된 복사본을 다시 원본에 할당합니다.
        OnStatChanged?.Invoke();
        Debug.Log($"{statType} 스탯이 {multiplier}배 만큼 적용! 현재 공격력: {TotalStats.AttackPower}");
    }
    // 최대 체력을 곱연산으로 강화하는 함수
    public void MultiplyMaxHealth(float multiplier)
    {
        var newStats = TotalStats;
        newStats.MaxHp = (int)(newStats.MaxHp * multiplier);
        TotalStats = newStats;
        Heal(TotalStats.MaxHp); // 체력도 최대치에 맞게 보정
        OnStatChanged?.Invoke();
    }
    // 최대 체력을 고정 수치만큼 감소시키는 함수 (악마 거래용)
    public void DecreaseMaxHealth(int amount)
    {
        var newStats = TotalStats;
        newStats.MaxHp -= amount;
        if (newStats.MaxHp < 1) newStats.MaxHp = 1; // 최소 1칸은 유지
        TotalStats = newStats;

        if (CurrentHp > TotalStats.MaxHp)
        {
            currentHp = TotalStats.MaxHp;
        }

        OnStatChanged?.Invoke();
    }
    /// <summary>
    /// 데미지를 받는 함수 (체력 1칸 감소)
    /// </summary>
    public void TakeDamage(int damage,Vector3 z)
    {
        currentHp -= damage;
        Debug.Log($"피해 {damage}! 남은 체력: {CurrentHp}");
        if (CurrentHp <= 0)
        {
            Die();
        }
    }
    // 최대 체력 대비 %로 회복하는 함수
    public void HealByPercentage(float percent)
    {
        int healAmount = (int)(TotalStats.MaxHp * percent);
        Heal(healAmount);
    }
    /// <summary>
    /// 체력을 회복하는 함수
    /// </summary>
    public void Heal(int amount)
    {
        currentHp += amount;
        if (CurrentHp > TotalStats.MaxHp)
        {
            currentHp = TotalStats.MaxHp;
        }
    }
    // 사망 관련 함수
    private void Die()
    {
        Lee.Scripts.GameManager.UI.ShowPopUpUI<GameOverUI>("Prefabs/UI/GameOverUI");
        Destroy(gameObject);
        // 사망 로직 추가
    }

}