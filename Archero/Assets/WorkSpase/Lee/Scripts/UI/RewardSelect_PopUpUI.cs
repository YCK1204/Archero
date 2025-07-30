using Lee.Scripts;
using System.Collections;
using System.Collections.Generic;
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

        void RandomReward()
        {




        }


        IEnumerator StartRountine()
        {
            yield return new WaitForSeconds(0.2f);
            Time.timeScale = 0f;


        }












    }
}
