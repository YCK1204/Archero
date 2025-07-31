using UnityEngine;

[System.Serializable]
public struct Stat
{
    public float Attack;
    // public int Defense; // ���� ����
    public int MaxHeartContainers; // �ִ� ü�� -> ��Ʈ ĭ �������� ����
    public float AttackSpeed;    // ���� �ӵ� ���� �߰�
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

    // Stat * Stat �� �����ϰ� ����� ������, ��� ���� �� �ڵ� ����
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