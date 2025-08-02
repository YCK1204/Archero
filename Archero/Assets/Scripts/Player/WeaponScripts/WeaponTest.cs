using UnityEngine;

public class WeaponTestLoader : MonoBehaviour
{
    public WeaponHolder weaponHolder;
    public WeaponData turretData;
    public GameObject turretPrefab;

    void Start()
    {
        // 2초 뒤 터렛 장착
        Invoke("EquipTurret", 4f);
    }

    void EquipTurret()
    {
        if (weaponHolder != null && turretData != null && turretPrefab != null)
        {
            weaponHolder.EquipWeapon(turretData, turretPrefab);
            Debug.Log("터렛 무기 장착 완료");
        }
        else
        {
            Debug.LogWarning("필요한 요소가 빠졌습니다");
        }
    }
}