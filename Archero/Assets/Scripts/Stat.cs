using UnityEngine;

[System.Serializable]
public class Stat
{
    [Header("기본 능력치")]
    public int maxHealth = 100;       // 최대 체력
    public int defense = 5;         // 방어력

    [Header("이동 능력치")]
    public float moveSpeed = 5f;     // 기본 이동 속도 (초기값 5f)
    public float dashSpeed = 15f;    // 대쉬 속도 (초기값 15f)
}