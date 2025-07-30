using UnityEngine;

[System.Serializable]
public struct Stat
{
    public int Attack;
    public int Defense;
    public int MaxHealth;
    public float MoveSpeed;
    public float DashSpeed;

    public Stat(int attack, int defense, int maxHealth, float moveSpeed, float dashSpeed)
    {
        Attack = attack;
        Defense = defense;
        MaxHealth = maxHealth;
        MoveSpeed = moveSpeed;
        DashSpeed = dashSpeed;
    }

    // Stat + Stat �� �����ϰ� ����� ������, ��� ���� �� �ڵ� ����
    public static Stat operator +(Stat a, Stat b)
    {
        return new Stat(
            a.Attack + b.Attack,
            a.Defense + b.Defense,
            a.MaxHealth + b.MaxHealth,
            a.MoveSpeed + b.MoveSpeed,
            a.DashSpeed + b.DashSpeed
        );
    }
    public static Stat operator -(Stat a, Stat b)
    {
        return new Stat(
            a.Attack - b.Attack,
            a.Defense - b.Defense,
            a.MaxHealth - b.MaxHealth,
            a.MoveSpeed - b.MoveSpeed,
            a.DashSpeed - b.DashSpeed
        );
    }
}