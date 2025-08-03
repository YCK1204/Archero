using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lee.Scripts
{
    public abstract class RewardSelectBaseUI : PopUpUI
    {
        [SerializeField] protected List<RectTransform> rewardSlots = new List<RectTransform>();
        protected List<StageRewardEntry> entries;
        protected Action<StageRewardEntry> onChosen;
        protected StageRewardEntry selectedEntry;

        protected override void Awake()
        {
            base.Awake();
            if (buttons.ContainsKey("DecideButton"))
            {
                buttons["DecideButton"].onClick.AddListener(OnDecideClicked);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            ClearSlots();
        }

        public virtual void Initialize(List<StageRewardEntry> entries, Action<StageRewardEntry> onChosen)
        {
            this.entries = entries;
            this.onChosen = onChosen;
            this.selectedEntry = null;
            StartCoroutine(OpenRoutine());
        }

        protected virtual IEnumerator OpenRoutine()
        {
            // 애니메이션이 있다면 대기
            yield return new WaitForSeconds(0.1f);

            // 슬롯 초기화 (자식 오브젝트들 제거)
            ClearSlots();

            // 보상 슬롯 생성
            CreateRewardSlots();
        }

        protected virtual void CreateRewardSlots()
        {
            for (int i = 0; i < entries.Count && i < rewardSlots.Count; i++)
            {
                var entry = entries[i];
                var slot = rewardSlots[i];

                if (entry.uiPrefab != null && slot != null)
                {
                    GameObject obj = GameManager.Resource.Instantiate(entry.uiPrefab, slot);
                    obj.transform.localPosition = Vector3.zero;

                    // 버튼이 있다면 해당 엔트리 선택 이벤트 연결
                    if (obj.TryGetComponent<Button>(out var btn))
                    {
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(() => selectedEntry = entry);
                    }
                }
            }
        }

        protected virtual void ClearSlots()
        {
            if (rewardSlots == null) return;

            foreach (var slot in rewardSlots)
            {
                if (slot != null)
                {
                    ClearSlotChildren(slot);
                }
            }
        }

        protected virtual void ClearSlotChildren(RectTransform slot)
        {
            if (slot == null) return;

            // 자식들을 배열로 복사해서 반복 중 수정 방지
            List<Transform> childrenToRemove = new List<Transform>();
            
            for (int i = 0; i < slot.childCount; i++)
            {
                Transform child = slot.GetChild(i);
                if (child != null)
                {
                    childrenToRemove.Add(child);
                }
            }

            // 모든 자식들을 한 번에 처리
            foreach (Transform child in childrenToRemove)
            {
                if (child != null)
                {
                    try
                    {
                        // 풀에서 관리되는 오브젝트는 풀에 반환
                        if (GameManager.Pool.IsContain(child.gameObject))
                        {
                            GameManager.Pool.Release(child.gameObject);
                        }
                        else
                        {
                            // 풀에서 관리되지 않는 오브젝트는 파괴
                            GameManager.Resource.Destroy(child.gameObject);
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogWarning($"자식 오브젝트 정리 중 오류 발생: {e.Message}");
                    }
                }
            }
        }

        protected virtual void OnDecideClicked()
        {
            var result = selectedEntry ?? (entries.Count > 0 ? entries[0] : null);
            // 버튼 클릭시 선택된 보상 적용
            onChosen?.Invoke(result);
            GameManager.UI.ClosePopUpUI();
        }
    }
} 