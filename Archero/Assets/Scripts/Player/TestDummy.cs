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
        if (Input.GetKeyDown(KeyCode.T) && !equipped)
        {
            weaponHolder.EquipWeapon(turretData, turretPrefab);
            Debug.Log("터렛 장착됨!");
            equipped = true;
        }
    }


}