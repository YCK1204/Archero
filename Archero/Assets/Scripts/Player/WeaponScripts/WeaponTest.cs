using UnityEngine;

public class WeaponTestLoader : MonoBehaviour
{
    public WeaponHolder weaponHolder;
    public WeaponData turretData;
    public GameObject turretPrefab;

    void Start()
    {
        // 2�� �� �ͷ� ����
        Invoke("EquipTurret", 4f);
    }

    void EquipTurret()
    {
        if (weaponHolder != null && turretData != null && turretPrefab != null)
        {
            weaponHolder.EquipWeapon(turretData, turretPrefab);
            Debug.Log("�ͷ� ���� ���� �Ϸ�");
        }
        else
        {
            Debug.LogWarning("�ʿ��� ��Ұ� �������ϴ�");
        }
    }
}