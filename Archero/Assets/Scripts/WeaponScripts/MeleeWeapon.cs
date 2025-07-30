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
            Debug.LogWarning("MeleeWeapon: Animator�� ������� �ʾҽ��ϴ�.");
            return;
        }

        animator.SetTrigger("Attack");

        // �ʿ� �� ���⼭ ��Ʈ�ڽ� Ȱ��ȭ �� �߰� ����
        // ex: EnableHitbox();
    }
}