using UnityEngine;

public class MeleeWeapon : WeaponBase
{
    [SerializeField] private Animator animator;

    public override void Init(WeaponData data)
    {
        base.Init(data);

    }

    protected override void PerformAttack()
    {
        if (animator == null)
        {
            Debug.LogWarning("MeleeWeapon: Animator가 연결되지 않았습니다.");
            return;
        }

        animator.SetTrigger("Attack");

        // 필요 시 여기서 히트박스 활성화 등 추가 가능
        // ex: EnableHitbox();
    }
}