using UnityEngine;

[System.Serializable]
public struct Stat
{
    public float AttackPower;
    // public int Defense; // ���� ����
    public int MaxHp; // 1 Heart per 2Hp
    public float AttackSpeed;    // ���� �ӵ� ���� �߰�
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

    // Stat * Stat �� �����ϰ� ����� ������, ��� ���� �� �ڵ� ����
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