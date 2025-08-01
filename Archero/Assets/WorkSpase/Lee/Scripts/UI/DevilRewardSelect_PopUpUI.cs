using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lee.Scripts
{

    public class DevilRewardSelect_PopUpUI : PopUpUI
    {
        [SerializeField] List<RectTransform> rewardSlots = new List<RectTransform>();
        private List<StageRewardEntry> entries;
        private Action<StageRewardEntry> onChosen;
        private StageRewardEntry selectedEntry;
        protected override void Awake()
        {
            base.Awake();
            buttons["DecideButton"].onClick.AddListener(() => { OnDecideClicked(); });
        }

        public void Initialize(List<StageRewardEntry> entries, Action<StageRewardEntry> onChosen)
        {
            this.entries = entries;
            this.onChosen = onChosen;
            this.selectedEntry = null;
            StartCoroutine(OpenRountine());
        }
        IEnumerator OpenRountine()
        {
            // 애니메이션이 있다면 초변경
            yield return new WaitForSeconds(0.1f);

            // 슬롯초기화 (자식오브젝트들 삭제)
            foreach (var slot in rewardSlots)
            {
                foreach (Transform child in slot)
                {
                    GameManager.Resource.Destroy(child.gameObject);
                }
            }

            for (int i = 0; i < entries.Count && i < rewardSlots.Count; i++)
            {
                var entry = entries[i];
                var slot = rewardSlots[i];

                if (entry.uiPrefab != null)
                {
                    GameObject obj = GameManager.Resource.Instantiate(entry.uiPrefab, slot);
                    obj.transform.localPosition = Vector3.zero;

                    // 보상을 선택할시 해당 보상에 대한 정보 가저오기
                    if (obj.TryGetComponent<Button>(out var btn))
                    {
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(() => selectedEntry = entry);
                    }
                }
            }
        }

        void OnDecideClicked()
        {
            var result = selectedEntry ?? (entries.Count > 0 ? entries[0] : null);
            // 버튼 클릭시 선택된 보상 적용
            onChosen?.Invoke(result);
            GameManager.UI.ClosePopUpUI();
        }
    }
}
