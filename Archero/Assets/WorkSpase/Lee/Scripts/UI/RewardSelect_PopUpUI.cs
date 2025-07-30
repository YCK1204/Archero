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
        protected override void Awake()
        {
            base.Awake();
            buttons["DecideButton"].onClick.AddListener(() => {  });
        }

        private void Start()
        {
            StartCoroutine(StartRountine());
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            Debug.Log("시간은 움직인다");
        }

        IEnumerator StartRountine()
        {
            yield return new WaitForSeconds(0.2f);
            curRewardList.Add(rectTransform["RewardSlot1"]);
            curRewardList.Add(rectTransform["RewardSlot2"]);
            curRewardList.Add(rectTransform["RewardSlot3"]);
            Time.timeScale = 0f;
            yield return new WaitForSeconds(0.2f);
            RandomReward();
        }

        private void RandomReward()
        {
            // 전체 섞기
            var allTypes = GameManager.Reward.AllRewardTypes.ToList();
            Shuffle(GameManager.Reward.AllRewardTypes.ToList());
            var picks = allTypes.Take(curRewardList.Count).ToArray();
            for (int i = 0; i < 3; i++)
            {
                var slotBtn = curRewardList[i];
                var slotTf = slotBtn.transform as RectTransform;

                foreach (Transform child in slotTf)
                    Destroy(child.gameObject);


                var data = GameManager.Reward.GetData(picks[i]);
                var go = GameManager.Resource.Instantiate(data.prefab);

  
                go.transform.SetParent(slotTf, false);// 로컬 포지션

                if (go.TryGetComponent<RectTransform>(out var rt))
                {
                    rt.position = slotTf.position;
                }
            }
        }

        private void Shuffle<T>(IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }












    }
}
