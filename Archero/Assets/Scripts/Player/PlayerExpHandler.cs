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

        // 외부 레벨업 이벤트 발행 (UI 등 연결 가능)
        OnLevelUp?.Invoke(playerLevel);
        // 스킬카드 UI 띄우기 (예: SkillManager 통해)
        Lee.Scripts.GameManager.Reward.ShowVkrReward();
        // 레벨업 이펙트 등 추가 가능
        Debug.Log($"레벨이 올랐따. 현재 레벨: {playerLevel}");
    }
}
