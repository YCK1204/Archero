using UnityEngine;

[System.Serializable]
public struct Stat
{
    public float AttackPower;
    // public int Defense; // 방어력 삭제
    public int MaxHp; // 1 Heart per 2Hp
    public float AttackSpeed;    // 공격 속도 스탯 추가
    public float MoveSpeed;
    public float DashSpeed;

    public Stat(float attackPower, int maxHp, float attackSpeed, float moveSpeed, float dashSpeed)
    {
        AttackPower = attackPower;
        MaxHp = maxHp;
        AttackSpeed = attackSpeed;
        MoveSpeed = moveSpeed;
        DashSpeed = dashSpeed;
    }

    // Stat * Stat 이 가능하게 만들어 레벨업, 장비 장착 시 코드 간결
    public static Stat operator *(Stat a, Stat b)
    {
        return new Stat(
            a.AttackPower * b.AttackPower,
            a.MaxHp * b.MaxHp,
            a.AttackSpeed * b.AttackSpeed,
            a.MoveSpeed * b.MoveSpeed,
            a.DashSpeed * b.DashSpeed
        );
    }
}