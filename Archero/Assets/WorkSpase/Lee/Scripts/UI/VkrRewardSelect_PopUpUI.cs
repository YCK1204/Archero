using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Lee.Scripts
{
    public class VkrRewardSelect_PopUpUI : PopUpUI
    {
        public List<RectTransform> curRewardList = new List<RectTransform>();
        CharacterStats characterStats;
        protected override void Awake()
        {
            base.Awake();
            characterStats = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterStats>();
        }

    }

}