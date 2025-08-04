using Assets.Define;
using UnityEngine;

public class TestDummy : MonoBehaviour
{
    private CharacterStats stats; // �Ǵ� ���� ���� ü�� ó�� Ŭ����

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
        Debug.Log($"���� �ǰݵ�! ���� ü��: asdf asdf ");
    }

    [SerializeField] private WeaponHolder weaponHolder;
    [SerializeField] private WeaponData turretData;
    [SerializeField] private GameObject turretPrefab;

    private bool equipped = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("T Ű �Է� �� �ͷ� ��ų ���� ����");

            var skillManager = SkillManager.Instance;
            if (skillManager == null)
            {
                Debug.LogWarning("SkillManager �ν��Ͻ��� ã�� �� �����ϴ�.");
                return;
            }

            // SummonTurret ��ų�� skillManager�� �����ͺ��̽����� ã�Ƽ� ����
            var turretSkill = skillManager
                .GetAllSkills()
                .Find(s => s.EffectID == "SummonTurret");

            if (turretSkill != null)
            {
                skillManager.SelectSkill(turretSkill);
            }
            else
            {
                Debug.LogWarning("SummonTurret ��ų�� ã�� ���߽��ϴ�.");
            }
        }
    }


}