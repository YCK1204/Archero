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
        public List<Button> curRewardList = new List<Button>();
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
            Time.timeScale = 0f;


        }

        private void RandomReward()
        {
            // 전체 섞기
            Shuffle(GameManager.Reward.AllRewardTypes.ToList());




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
