using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WeaponData")]
public class WeaponData : ScriptableObject  // ���� �⺻ ������ 
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

    [Header("����ü ��ų ȿ��")]
    [SerializeField] private List<EProjectileModifier> modifiers = new();

    /// <summary>
    /// ==================== ������ �����̽� ===================
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

    // --- Modifier ���� ---
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