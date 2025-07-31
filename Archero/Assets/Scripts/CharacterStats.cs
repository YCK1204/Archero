using Lee.Scripts;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("기본 스탯 설정")]

    [SerializeField] private Stat baseStats;
    // 현재 능력치 (실제 게임에서 변동되는 값들)
    public Stat TotalStats { get; private set; }

    [Header("캐릭터 정보")]
    public int Level;
    public int CurrentHealth;
    public int FinalAttack;
    public int FinalDefense;
    public float MoveSpeed;
    public float DashSpeed;

    void Awake()
    {
        // 게임 시작 시, 최종 스탯을 기본 스탯으로 초기화
        // new Stat(공격력, 방어력, 최대체력, 이동속도, 대쉬속도)
        baseStats = new Stat(1, 5, 100, 5f, 15f);
        TotalStats = baseStats;
        CurrentHealth = TotalStats.MaxHealth;
        FinalAttack = TotalStats.Attack;
        FinalDefense = TotalStats.Defense;
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
    /// 최종 방어력을 업데이트하는 함수. 방어구 장착/해제 스크립트에서 이 함수를 호출
    /// </summary>
    /// <param name="newDefense">최종 방어력</param>
    public void UpdateFinalDefense(int newDefense)
    {
        FinalDefense = newDefense;
        // 곱연산으로
    }
    public void LevelUp()
    {
        Level++;
        Stat levelUpBonus = new Stat(2, 1, 10, 0f, 0f); // 레벨업 시 오르는 스탯 ( 공격력, 방어력, 체력 )
        TotalStats += levelUpBonus;

        // 체력 3분의 1 회복
        if (CurrentHealth + TotalStats.MaxHealth / 3 < TotalStats.MaxHealth)
        {
            CurrentHealth += TotalStats.MaxHealth / 3;
        }
        else
        {
            CurrentHealth = TotalStats.MaxHealth;
        }
        Lee.Scripts.GameManager.UI.ShowPopUpUI<RewardSelect_PopUpUI>("Prefabs/UI/RewardUI");
    }

    /// <summary>
    /// 데미지를 받는 함수
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    public void TakeDamage(int damage)
    {
        // 1. 방어력을 고려한 최종 데미지 계산
         int finalDamage = damage - TotalStats.Defense;

        // 2. 최소 1의 데미지는 받도록 보정
        if (finalDamage < 1)
        {
            finalDamage = 1;
        }

        // 3. 현재 체력에서 최종 데미지만큼 실제 감소 적용
        CurrentHealth -= finalDamage;

        // 4. 데미지를 받은 후 남은 체력을 로그로 출력하여 확인
        Debug.Log($"[데미지 계산] {gameObject.name}이(가) {finalDamage}의 데미지를 입었습니다. 남은 체력: {CurrentHealth}");

        // 5. 체력이 0보다 아래로 내려가지 않도록 보정
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            // 여기에 캐릭터 사망 로직 구현
            Debug.Log(gameObject.name + "이(가) 사망했습니다.");
        }
    }
}