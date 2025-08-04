using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("�ʱ� ���� ���� ����")]
    [SerializeField] private WeaponData startingWeaponData;
    [SerializeField] private GameObject startingWeaponPrefab;

    private List<WeaponBase> equippedWeapons = new();
    private CharacterStats ownerStats;

    private void Awake()
    {
        ownerStats = GetComponentInParent<CharacterStats>();
    }
    private void Start()
    {
        //yield return null;           // ���뤷��������������
        EquipWeapon(startingWeaponData, startingWeaponPrefab);
    }


    private void Update()
    {
        foreach (var weapon in equippedWeapons)
        {
            weapon?.Activate(); // ���Ⱑ ������ ����/ȸ��/ƽ�� �� ó��
        }
    }


    /// <summary>
    /// ��ų ī�� ȹ�� ������ ���� ����
    /// </summary>
    public void EquipWeapon(WeaponData data, GameObject prefab)
    {
        if (data == null || prefab == null)
        {
            Debug.LogWarning("WeaponHolder: ���� �����ͳ� �������� null�Դϴ�.");
            return;
        }

        GameObject weaponObj = Instantiate(prefab, transform);
        WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();

        if (weapon == null)
        {
            Debug.LogError("WeaponHolder: �����տ� WeaponBase�� �����ϴ�.");
            Destroy(weaponObj);
            return;
        }

        weapon.Init(data, ownerStats, this);
        equippedWeapons.Add(weapon);
    }

    /// <summary>
    /// ��ü ���� ���� (��: ���? Ȥ�� �� é�� ����? ��ư ���¿� ��������)
    /// </summary>
    public void ClearWeapons()
    {
        foreach (var weapon in equippedWeapons)
        {
            if (weapon != null)
                Destroy(weapon.gameObject);
        }
        equippedWeapons.Clear();
    }

    /// <summary>
    /// ���� ����� �� �ν� ����
    /// </summary>
    public Transform FindNearestMonster(float radius)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask("Monster"));
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var hit in hits)
        {
            float dist = Vector2.Distance(transform.position, hit.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = hit.transform;
            }
        }

        return nearest;
    }
}