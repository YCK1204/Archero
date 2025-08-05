using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpHandler : MonoBehaviour
{
    private CharacterStats stats;

    [SerializeField] private int playerLevel = 1;
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int baseExpToLevelUp = 10;
    [SerializeField] private float expGrowthFactor = 1.5f;

    public int CurrentPlayerLevel => playerLevel;
    public int CurrentExp => currentExp;
    public int RequiredExp => Mathf.FloorToInt(baseExpToLevelUp * Mathf.Pow(expGrowthFactor, playerLevel - 1));

    public event Action<int> OnLevelUp;

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    public void GainExp(int amount)
    {
        currentExp += amount;
        while (currentExp >= RequiredExp)
        {
            currentExp -= RequiredExp;
            LevelUp();
        }
    }

    private void LevelUp()
    {
        playerLevel++;

        // �ܺ� ������ �̺�Ʈ ���� (UI �� ���� ����)
        OnLevelUp?.Invoke(playerLevel);
        // ��ųī�� UI ���� (��: SkillManager ����)
        Lee.Scripts.GameManager.Reward.ShowVkrReward();
        // ������ ����Ʈ �� �߰� ����
        Debug.Log($"������ �ö���. ���� ����: {playerLevel}");
    }
}
