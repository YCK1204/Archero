using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Lee.Scripts
{
    public class RewardSelect_PopUpUI : PopUpUI
    {
        public List<RectTransform> curRewardList = new List<RectTransform>();
         RewardData selectedReward;// ���� ���� ����
        CharacterStats characterStats;
        protected override void Awake()
        {
            base.Awake();
            characterStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();



            buttons["DecideButton"].onClick.AddListener(() => { OnDecideClicked(); });
        }

        private void OnEnable()
        {
            StartCoroutine(OpenRountine());
        }

        IEnumerator OpenRountine()
        {
            yield return new WaitForSeconds(0.2f);
            curRewardList.Add(rectTransform["RewardSlot1"]);
            curRewardList.Add(rectTransform["RewardSlot2"]);
            curRewardList.Add(rectTransform["RewardSlot3"]);
            yield return new WaitForSeconds(0.2f);
            var picks = GameManager.Reward.GetRandomRewards(curRewardList.Count);
            for (int i = 0; i < picks.Count; i++)
            {
                var data = picks[i];
                var slotTf = curRewardList[i].transform as RectTransform;

                foreach (Transform c in slotTf) Destroy(c.gameObject);             // ������ ����
                var go = GameManager.Resource.Instantiate(data.prefab);
                go.transform.SetParent(slotTf, false); // ��������Ʈ

                if (go.TryGetComponent<Button>(out var btn))
                {
                    btn.onClick.RemoveAllListeners();       // ��ư �̺�Ʈ �ʱ�ȭ
                    btn.onClick.AddListener(() =>{selectedReward = data;});  // ���� Ŭ���� �ش� ���� ���� ������ �Է�
                }
            }
        }

        private void OnDecideClicked()
        {
            if (selectedReward is StatRewardData statData)
            {
                // ���� ���ʽ�
                //  int current = characterStats.(statData.statType); ĳ���� ���ȿ��� �ɷ�ġ ��������
                // int gain = GameManager.Reward.CalculateStatBonus(statData, current);  ĳ���� ���� ���ʽ� ��ġ ����
                // characterStats.AddStat(statData.statType, gain); ĳ���� ���� ���ʽ� �߰�
                Debug.Log($"�̰���{statData}�Դϴ�");
            }
            else if(selectedReward is SkillRewardData skillData)
            {
                // ��ų ���� ����
                Debug.Log($"�̰���{skillData}�Դϴ�");
            }

            gameObject.SetActive(false);
            //GameManager.UI.ClosePopUpUI();
        }
    }
}
