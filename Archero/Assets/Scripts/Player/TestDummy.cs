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
        if (Input.GetKeyDown(KeyCode.T) && !equipped)
        {
            weaponHolder.EquipWeapon(turretData, turretPrefab);
            Debug.Log("�ͷ� ������!");
            equipped = true;
        }
    }


}