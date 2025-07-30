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

    // Stat + Stat 이 가능하게 만들어 레벨업, 장비 장착 시 코드 간결
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