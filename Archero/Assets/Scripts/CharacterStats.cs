using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("�⺻ ���� ����")]
    [SerializeField] private Stat baseStats;

    // ���� �ɷ�ġ (���� ���ӿ��� �����Ǵ� ����)
    public int CurrentHealth { get; private set; }
    public int FinalAttack { get; private set; } // ���� ���ݷ� (����, ���� ���� ��� ����)
    public int FinalDefense { get; private set; }
    public float FinalMoveSpeed { get; private set; }
    public float FinalDashSpeed { get; private set; }

    void Awake()
    {
        // ���� ���� ��, �⺻ ������ �������� ���� �ɷ�ġ�� �ʱ�ȭ
        CurrentHealth = baseStats.maxHealth;
        FinalDefense = baseStats.defense;
        FinalMoveSpeed = baseStats.moveSpeed;
        FinalDashSpeed = baseStats.dashSpeed;

        // ���� ���ݷ��� �ϴ� 1���� ���� (���� ���� �� ������Ʈ ����)
        FinalAttack = 1;
    }

    /// <summary>
    /// ���� ���ݷ��� ������Ʈ�ϴ� �Լ�. ���� ����/���� ��ũ��Ʈ���� �� �Լ��� ȣ��
    /// </summary>
    /// <param name="newAttack">���� ���ݷ�</param>
    public void UpdateFinalAttack(int newAttack)
    {
        FinalAttack = newAttack;
        // ����������
    }

    /// <summary>
    /// �������� �޴� �Լ�
    /// </summary>
    /// <param name="damage">���� ������</param>
    public void TakeDamage(int damage)
    {
        // ������ ����� ���� ������ ���
        int finalDamage = damage - FinalDefense;
        if (finalDamage < 1) finalDamage = 1; // �ּ� 1�� �������� �޵��� ����

        CurrentHealth -= finalDamage;
        
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            // ���⿡ ĳ���� ��� ����
        }
    }
}