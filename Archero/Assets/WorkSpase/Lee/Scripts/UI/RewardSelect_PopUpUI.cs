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
         RewardData selectedReward;// 최종 선택 보상
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

                foreach (Transform c in slotTf) Destroy(c.gameObject);             // 기존꺼 삭제
                var go = GameManager.Resource.Instantiate(data.prefab);
                go.transform.SetParent(slotTf, false); // 로컬포인트

                if (go.TryGetComponent<Button>(out var btn))
                {
                    btn.onClick.RemoveAllListeners();       // 버튼 이벤트 초기화
                    btn.onClick.AddListener(() =>{selectedReward = data;});  // 보상 클릭시 해당 보상에 대한 데이터 입력
                }
            }
        }

        private void OnDecideClicked()
        {
            if (selectedReward is StatRewardData statData)
            {
                // 스탯 보너스
                //  int current = characterStats.(statData.statType); 캐릭터 스탯에서 능력치 가저오기
                // int gain = GameManager.Reward.CalculateStatBonus(statData, current);  캐릭터 스탯 보너스 수치 연산
                // characterStats.AddStat(statData.statType, gain); 캐릭터 스탯 보너스 추가
                Debug.Log($"이것은{statData}입니다");
            }
            else if(selectedReward is SkillRewardData skillData)
            {
                // 스킬 적용 로직
                Debug.Log($"이것은{skillData}입니다");
            }

            gameObject.SetActive(false);
            //GameManager.UI.ClosePopUpUI();
        }
    }
}
