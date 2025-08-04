using Assets.Define;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    private CharacterStats stats; // 또는 몬스터 전용 체력 처리 클래스

    private void Awake()
    {
        stats = GetComponent<CharacterStats>();
    }

    private void OnEnable()
    {
        var col = GetComponent<Collider2D>();
        BattleManager.GetInstance.RegistHitInfo(col, OnHit);
    }

    private void OnDisable()
    {
        var col = GetComponent<Collider2D>();
        BattleManager.GetInstance.RemoveHitInfo(col);
    }

    private void OnHit(int damage, Vector3 attackerPos)
    {
        //stats.TakeDamage(damage, attackerPos);
        Debug.Log($"몬스터 피격됨! 현재 체력: asdf asdf ");
    }

    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private WeaponData turretData;
    [SerializeField] private GameObject turretPrefab;

    private bool equipped = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T 키 입력 → 터렛 스킬 강제 선택");

            var skillManager = SkillManager.Instance;
            if (skillManager == null)
            {
                Debug.LogWarning("SkillManager 인스턴스를 찾을 수 없습니다.");
                return;
            }

            // SummonTurret 스킬을 skillManager의 데이터베이스에서 찾아서 선택
            var turretSkill = skillManager
                .GetAllSkills()
                .Find(s => s.EffectID == "SummonTurret");

            if (turretSkill != null)
            {
                skillManager.SelectSkill(turretSkill);
            }
            else
            {
                Debug.LogWarning("SummonTurret 스킬을 찾지 못했습니다.");
            }
        }
    }


}