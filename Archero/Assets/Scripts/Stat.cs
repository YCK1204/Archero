using UnityEngine;

[System.Serializable]
public struct Stat
{
    public float Attack;
    // public int Defense; // 방어력 삭제
    public int MaxHeartContainers; // 최대 체력 -> 하트 칸 개념으로 변경
    public float AttackSpeed;    // 공격 속도 스탯 추가
    public float MoveSpeed;
    public float DashSpeed;

    public Stat(float attack, int maxHeartContainers, float attackSpeed, float moveSpeed, float dashSpeed)
    {
        Attack = attack;
        MaxHeartContainers = maxHeartContainers;
        AttackSpeed = attackSpeed;
        MoveSpeed = moveSpeed;
        DashSpeed = dashSpeed;
    }

    // Stat * Stat 이 가능하게 만들어 레벨업, 장비 장착 시 코드 간결
    public static Stat operator *(Stat a, Stat b)
    {
        return new Stat(
            a.Attack * b.Attack,
            a.MaxHeartContainers * b.MaxHeartContainers,
            a.AttackSpeed * b.AttackSpeed,
            a.MoveSpeed * b.MoveSpeed,
            a.DashSpeed * b.DashSpeed
        );
    }
}