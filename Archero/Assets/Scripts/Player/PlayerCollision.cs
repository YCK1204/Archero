using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("������ ����")]
    [SerializeField] private int attackDamage = 5;      // ���Ͱ� �÷��̾�� ���� ������ ��
    [SerializeField] private float damageInterval = 2f; // �������� �ִ� �ð� ���� (2��)

    // ���� ���� ���� ������ �ڷ�ƾ�� �����ϱ� ���� ����
    private Coroutine damageCoroutine;

    /// <summary>
    /// �ٸ� Collider�� �浹�� ������ �� �ڵ����� ȣ��Ǵ� �Լ�
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterStats playerStats = collision.gameObject.GetComponent<CharacterStats>();

            if (playerStats != null && damageCoroutine == null)
            {
                // DealDamageOverTime �ڷ�ƾ�� �����ϰ�, ���� ���� �ڷ�ƾ ������ ����
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerStats));
            }
        }
    }

    /// <summary>
    /// �ٸ� Collider�� �浹�� ������ �� �ڵ����� ȣ��Ǵ� �Լ�
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���� ���� ������ �ڷ�ƾ�� �ִٸ� ����
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                // �ڷ�ƾ ������ null�� �ʱ�ȭ�Ͽ� �ٽ� �ڷ�ƾ�� ������ �� �ֵ��� ��
                damageCoroutine = null;
                Debug.Log("�÷��̾���� �浹�� ���� ���� �������� �����մϴ�.");
            }
        }
    }

    /// <summary>
    /// ������ �ð� �������� �������� �������� �ִ� �ڷ�ƾ
    /// </summary>
    /// <param name="playerStats">�������� ���� �÷��̾��� ����</param>
    private IEnumerator DealDamageOverTime(CharacterStats playerStats)
    {
        Debug.Log("�÷��̾�� �浹! �������� �Խ��ϴ�.");

        // �� �ڷ�ƾ�� ����Ǵ� ���� ���� �ݺ�
        while (true)
        {
            // 1. ���� �������� �� �� �ְ�
            playerStats.TakeDamage(attackDamage);

            // 2. damageInterval ��ŭ ��ٸ� (��: 2��)
            yield return new WaitForSeconds(damageInterval);
        }
    }
}