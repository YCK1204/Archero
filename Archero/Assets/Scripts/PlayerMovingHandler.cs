using UnityEngine;
using System.Collections;

public class PlayerMovingHandler : MonoBehaviour
{
    //[SerializeField] private float moveSpeed = 5f; 
    //[SerializeField] private float dashSpeed = 15f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private float acceleration = 15f;

    private bool isDashing = false;
    private float currentSpeed; // 현재 적용 중인 이동 속도 (moveSpeed 또는 dashSpeed 사이를 부드럽게 전환)
    private float lastDashTime = -Mathf.Infinity;
    private Rigidbody2D rb;
    private CharacterStats characterStats;

    // public bool IsInvincible { get; private set; } = false;       // 임시로 만든 대쉬중무적 불값

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterStats = GetComponent<CharacterStats>();
        currentSpeed = characterStats.TotalStats.MoveSpeed;
    }

    public void HandleMove(Vector2 input)
    {
        float targetSpeed = isDashing ? characterStats.TotalStats.DashSpeed : characterStats.TotalStats.MoveSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * acceleration); // 현재 속도를 목표 속도로 부드럽게 보간
        rb.velocity = input * currentSpeed; // 실제 이동
    }

    public bool CanDash(Vector2 input)
    {
        return !isDashing && input != Vector2.zero && Time.time >= lastDashTime + dashCooldown;
    }

    public void TryDash()
    {
        lastDashTime = Time.time;
        StartCoroutine(DashRoutine());             // 대쉬 루틴(코루틴) 시작
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;
      //  IsInvincible = true;
        yield return new WaitForSeconds(dashDuration); // 일정 시간 동안 대쉬 유지

        isDashing = false;
      //  IsInvincible = false;
        yield break;
    }
}