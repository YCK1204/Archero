using UnityEngine;

[System.Serializable]
public class Stat
{
    [Header("�⺻ �ɷ�ġ")]
    public int maxHealth = 100;       // �ִ� ü��
    public int defense = 5;         // ����

    [Header("�̵� �ɷ�ġ")]
    public float moveSpeed = 5f;     // �⺻ �̵� �ӵ� (�ʱⰪ 5f)
    public float dashSpeed = 15f;    // �뽬 �ӵ� (�ʱⰪ 15f)
}