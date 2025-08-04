using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject  // 무기 기본 데이터 
{
    [Header("Basic Data")]
    [SerializeField] private string weaponName;
    [SerializeField] private float attackPower;
    [SerializeField] private float cooldown;

    [Header("KnockBack info")]
    [SerializeField] private float knockbackPower;
    [SerializeField] private float knockbackTime;


    [Header("Projectile Only")]
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float range;
    [SerializeField] private int projectileCount;
    [SerializeField] private float spreadAngle = 10f;

    [Header("투사체 스킬 효과")]
    [SerializeField] private List<EProjectileModifier> modifiers = new();

    /// <summary>
    /// ==================== 윤님의 어드바이스 ===================
    /// </summary>
    public string WeaponName => weaponName;
    public float AttackPower => attackPower;
    public float Cooldown => cooldown;
    public float KnockbackPower => knockbackPower;
    public float KnockbackTime => knockbackTime;
    public float ProjectileSpeed => projectileSpeed;
    public float Range => range;
    public int ProjectileCount => projectileCount;
    public float SpreadAngle => spreadAngle;
    public IReadOnlyList<EProjectileModifier> Modifiers => modifiers;

    // --- Modifier 조작 ---
    public void AddModifier(EProjectileModifier mod)
    {
        if (!modifiers.Contains(mod))
            modifiers.Add(mod);
    }

    public bool HasModifier(EProjectileModifier mod)
    {
        return modifiers.Contains(mod);
    }

    public void IncreaseProjectileCount(int amount)
    {
        projectileCount += amount;
    }
}