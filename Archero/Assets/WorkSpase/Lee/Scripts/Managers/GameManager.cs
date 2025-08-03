using Assets.Define;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Lee.Scripts
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get { return instance; } }


        // Managers=========================
        private static UIManager uiManager;
        public static UIManager UI { get { return uiManager; } }

        private static ResourceManager resourceManager;
        public static ResourceManager Resource { get { return resourceManager; } }

        private static PoolManager poolManager;
        public static PoolManager Pool { get { return poolManager; } }

        private static RewardManager rewardManager;
        public static RewardManager Reward { get { return rewardManager; } }

        private static SkillManager skillManager;
        public static SkillManager SkillReward { get { return skillManager; } }


        // Clear변수
        int clearCount = 23;
        public int _clearCount { get => clearCount; set => clearCount = value; }



        private void Awake()
        {
            if (instance != null)
            {
                Destroy(this);
                return;
            }

            instance = this;
            DontDestroyOnLoad(this);
            InitManagers();
        }

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
        private void InitManagers()
        {
            GameObject resourceObj = new GameObject();
            resourceObj.name = "ResourceManager";
            resourceObj.transform.parent = transform;
            resourceManager = resourceObj.AddComponent<ResourceManager>();

            GameObject poolObj = new GameObject();
            poolObj.name = "PoolManager";
            poolObj.transform.parent = transform;
            poolManager = poolObj.AddComponent<PoolManager>();

            GameObject uiObj = new GameObject();
            uiObj.name = "UIManager";
            uiObj.transform.parent = transform;
            uiManager = uiObj.AddComponent<UIManager>();

            GameObject reObj = new GameObject();
            reObj.name = "RewardManager";
            reObj.transform.parent = transform;
            rewardManager = reObj.AddComponent<RewardManager>();

            GameObject skillObj = new GameObject();
            skillObj.name = "SkillManager";
            skillObj.transform.parent = transform;
            skillManager = skillObj.AddComponent<SkillManager>();
        }

        public void CheckStageClear()
        {
            var unitDict = BattleManager.GetInstance.GetUnitDIct;
            Debug.Log($"몬스터{unitDict}");
            if (unitDict.Count == 1)
            {
                Debug.Log($"클리어 카운터{clearCount}");
                var last = unitDict.Last().Key.gameObject;

                if (last.CompareTag("Monster"))
                {
                    uiManager.ShowPopUpUI<GameOverUI>("Prefabs/UI/GameOverUI");  // UI 만들고 수정
                    return;
                }

                if (last.CompareTag("Player"))
                {
                    clearCount--;
                    if (clearCount > 0)
                    {
                        Debug.Log($"클리어 카운터{clearCount}");
                        int roomsCleared = 23 - clearCount;
                        int[] groupSizes = { 3, 1, 2, 1, 1 };
                        ESkillCategory[] categories = { ESkillCategory.LevelUp,   ESkillCategory.Valkyrie, ESkillCategory.LevelUp,    ESkillCategory.Angel,   ESkillCategory.Devil};
                        int cycleLength = groupSizes.Sum();
                        int posInCycle = (roomsCleared - 1) % cycleLength;
                        int cumulative = 0, groupIndex = 0;
                        for (int i = 0; i < groupSizes.Length; i++)
                        {
                            cumulative += groupSizes[i];
                            if (posInCycle < cumulative)
                            {
                                groupIndex = i;
                                break;
                            }
                        }
                        int pickCount = groupSizes[groupIndex];
                        ESkillCategory cat = categories[groupIndex];
                        var grades = Enum.GetValues(typeof(ESkillGrade)).Cast<ESkillGrade>().ToArray();
                        var randomGrade = grades[UnityEngine.Random.Range(0, grades.Length)];
                        Reward.ShowReward(randomGrade,  cat, pickCount);
                    }
                    else
                    {
                        GameManager.UI.ShowPopUpUI<ClearGameUI>("Prefabs/UI/ClearGameUI");
                    }

                }

            }
        }

    }

}


