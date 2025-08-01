using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolder : MonoBehaviour
{
    [Header("초기 장착 무기 설정")]
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
        //yield return null;           // 으헝ㅇ으엉ㅇ으어허헝
        EquipWeapon(startingWeaponData, startingWeaponPrefab);
    }


    private void Update()
    {
        foreach (var weapon in equippedWeapons)
        {
            weapon?.Activate(); // 무기가 스스로 공격/회전/틱딜 등 처리
        }
    }


    /// <summary>
    /// 스킬 카드 획득 등으로 무기 장착
    /// </summary>
    public void EquipWeapon(WeaponData data, GameObject prefab)
    {
        if (data == null || prefab == null)
        {
            Debug.LogWarning("WeaponHolder: 무기 데이터나 프리팹이 null입니다.");
            return;
        }

        GameObject weaponObj = Instantiate(prefab, transform);
        WeaponBase weapon = weaponObj.GetComponent<WeaponBase>();

        if (weapon == null)
        {
            Debug.LogError("WeaponHolder: 프리팹에 WeaponBase가 없습니다.");
            Destroy(weaponObj);
            return;
        }

        weapon.Init(data, ownerStats, this);
        equippedWeapons.Add(weapon);
    }

    /// <summary>
    /// 전체 무기 제거 (예: 사망? 혹은 뭐 챕터 포기? 여튼 리셋에 쓰시지요)
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
}