using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("기본 스탯 설정")]
    [SerializeField] private Stat baseStats;

    // 현재 능력치 (실제 게임에서 변동되는 값들)
    public int CurrentHealth { get; private set; }
    public int FinalAttack { get; private set; } // 최종 공격력 (무기, 버프 등이 모두 계산된)
    public int FinalDefense { get; private set; }
    public float FinalMoveSpeed { get; private set; }
    public float FinalDashSpeed { get; private set; }

    void Awake()
    {
        // 게임 시작 시, 기본 스탯을 바탕으로 현재 능력치를 초기화
        CurrentHealth = baseStats.maxHealth;
        FinalDefense = baseStats.defense;
        FinalMoveSpeed = baseStats.moveSpeed;
        FinalDashSpeed = baseStats.dashSpeed;

        // 최종 공격력은 일단 1으로 시작 (무기 장착 시 업데이트 예정)
        FinalAttack = 1;
    }

    /// <summary>
    /// 최종 공격력을 업데이트하는 함수. 무기 장착/해제 스크립트에서 이 함수를 호출
    /// </summary>
    /// <param name="newAttack">최종 공격력</param>
    public void UpdateFinalAttack(int newAttack)
    {
        FinalAttack = newAttack;
        // 곱연산으로
    }

    /// <summary>
    /// 데미지를 받는 함수
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    public void TakeDamage(int damage)
    {
        // 방어력을 고려한 최종 데미지 계산
        int finalDamage = damage - FinalDefense;
        if (finalDamage < 1) finalDamage = 1; // 최소 1의 데미지는 받도록 설정

        CurrentHealth -= finalDamage;
        
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            // 여기에 캐릭터 사망 구현
        }
    }
}