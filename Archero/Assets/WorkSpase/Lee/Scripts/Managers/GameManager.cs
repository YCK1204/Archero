using Assets.Define;
using System.Collections;
using System.Collections.Generic;
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

        // Clear변수
        int clearCount = 0;
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
        }

        public void CheckStageClear()
        {
            var unitDict = BattleManager.GetInstance.GetUnitDIct;
     
            if (unitDict.Count == 1)
            {
                var last = unitDict.Last().Key.gameObject;
                if (last.CompareTag("Player"))
                {
                    uiManager.ShowPopUpUI<PopUpUI>("//");  // UI 만들고 수정
                }
                else if (last.CompareTag("Monster"))
                {
                    uiManager.ShowPopUpUI<PopUpUI>("//");  // UI 만들고 수정
                    clearCount--;
                    if(clearCount == 0)
                    {
                        // UI 만들고 수정
                    }
                }
            }

        }
    }
}

