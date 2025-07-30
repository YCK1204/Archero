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

        private void OnDisable()
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

        }

    }
}
