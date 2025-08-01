using System.Collections;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [Header("데미지 설정")]
    [SerializeField] private int attackDamage = 5;      // 몬스터가 플레이어에게 입힐 데미지 양
    [SerializeField] private float damageInterval = 2f; // 데미지를 주는 시간 간격 (2초)

    // 현재 실행 중인 데미지 코루틴을 저장하기 위한 변수
    private Coroutine damageCoroutine;

    /// <summary>
    /// 다른 Collider와 충돌을 시작할 때 자동으로 호출되는 함수
    /// </summary>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterStats playerStats = collision.gameObject.GetComponent<CharacterStats>();

            if (playerStats != null && damageCoroutine == null)
            {
                // DealDamageOverTime 코루틴을 시작하고, 실행 중인 코루틴 정보를 저장
                damageCoroutine = StartCoroutine(DealDamageOverTime(playerStats));
            }
        }
    }

    /// <summary>
    /// 다른 Collider와 충돌이 끝났을 때 자동으로 호출되는 함수
    /// </summary>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 실행 중인 데미지 코루틴이 있다면 중지
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                // 코루틴 변수를 null로 초기화하여 다시 코루틴을 시작할 수 있도록 함
                damageCoroutine = null;
                Debug.Log("플레이어와의 충돌이 끝나 지속 데미지를 중지합니다.");
            }
        }
    }

    /// <summary>
    /// 지정된 시간 간격으로 지속적인 데미지를 주는 코루틴
    /// </summary>
    /// <param name="playerStats">데미지를 받을 플레이어의 스탯</param>
    private IEnumerator DealDamageOverTime(CharacterStats playerStats)
    {
        Debug.Log("플레이어와 충돌! 데미지를 입습니다.");

        // 이 코루틴이 실행되는 동안 무한 반복
        while (true)
        {
            // 1. 먼저 데미지를 한 번 주고
            playerStats.TakeDamage(attackDamage);

            // 2. damageInterval 만큼 기다림 (예: 2초)
            yield return new WaitForSeconds(damageInterval);
        }
    }
}