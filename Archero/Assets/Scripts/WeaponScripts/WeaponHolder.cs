using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("�ʱ� ���� ���� ����")]
    [SerializeField] private WeaponData startingWeaponData;
    [SerializeField] private GameObject startingWeaponPrefab;

    private List<WeaponBase> equippedWeapons = new List<WeaponBase>();

    private void Start()
    {
        // ���� ���� �� �ʱ� ���� ����
        EquipWeapon(startingWeaponData, startingWeaponPrefab);
    }

    private void Update()
    {
        RotateTowardNearestMonster(GetMaxWeaponRange());
        foreach (var weapon in equippedWeapons)
        {
            if (weapon != null)
                weapon.TickAttack(); // �� ������ �ڵ� ���� ó��
        }
    }

    /// <summary>
    /// ���⸦ �߰� �����ϴ� �޼��� (��ų ī�� ��� ȣ���)
    /// </summary>
    public void EquipWeapon(WeaponData data, GameObject prefab)
    {
        if (data == null || prefab == null)
        {
            Debug.LogWarning("WeaponHolder: ���� ������ �Ǵ� �������� null�Դϴ�.");
            return;
        }

        GameObject weaponObj = Instantiate(prefab, transform); // ���⸦ Header �ڽ����� ����
        WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();

        if (weapon == null)
        {
            Debug.LogError("WeaponHolder: ���� �����տ� WeaponBase�� ����");
            Destroy(weaponObj);
            return;
        }

        weapon.Init(data);
        equippedWeapons.Add(weapon);
    }

    /// <summary>
    /// ���� ������ ��� ���⸦ ����
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

    private float GetMaxWeaponRange()          // ���� �������� ��ų�� ��� ���� �߿��� ���� �� ��Ÿ��� ���� ���⸦ ã�ƿ�
    {
        float max = 0f;
        foreach (var w in equippedWeapons)
        {
            if (w != null)
                max = Mathf.Max(max, w.WeaponData.Range);
        }
        return max;
    }
    /// <summary>
    /// ȸ�� ����
    /// </summary>
    private void RotateTowardNearestMonster(float radius)
    {
        Transform target = FindNearestMonster(radius);
        if (target == null) return;

        Vector2 dir = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}