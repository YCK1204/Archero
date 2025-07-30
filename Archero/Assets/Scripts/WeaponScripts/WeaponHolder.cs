using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("초기 장착 무기 설정")]
    [SerializeField] private WeaponData startingWeaponData;
    [SerializeField] private GameObject startingWeaponPrefab;

    private List<WeaponBase> equippedWeapons = new List<WeaponBase>();

    private void Start()
    {
        // 게임 시작 시 초기 무기 장착
        EquipWeapon(startingWeaponData, startingWeaponPrefab);
    }

    private void Update()
    {
        RotateTowardNearestMonster(GetMaxWeaponRange());
        foreach (var weapon in equippedWeapons)
        {
            if (weapon != null)
                weapon.TickAttack(); // 각 무기의 자동 공격 처리
        }
    }

    /// <summary>
    /// 무기를 추가 장착하는 메서드 (스킬 카드 등에서 호출됨)
    /// </summary>
    public void EquipWeapon(WeaponData data, GameObject prefab)
    {
        if (data == null || prefab == null)
        {
            Debug.LogWarning("WeaponHolder: 무기 데이터 또는 프리팹이 null입니다.");
            return;
        }

        GameObject weaponObj = Instantiate(prefab, transform); // 무기를 Header 자식으로 생성
        WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();

        if (weapon == null)
        {
            Debug.LogError("WeaponHolder: 무기 프리팹에 WeaponBase가 없음");
            Destroy(weaponObj);
            return;
        }

        weapon.Init(data);
        equippedWeapons.Add(weapon);
    }

    /// <summary>
    /// 현재 장착된 모든 무기를 제거
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
    /// 가장 가까운 적 인식 로직
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

    private float GetMaxWeaponRange()          // 현재 보유중인 스킬의 모든 무기 중에서 가장 긴 사거리를 가진 무기를 찾아옴
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
    /// 회전 로직
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