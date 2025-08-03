using System;
using System.Collections;
using UnityEngine;

public class PlayerMovingHandler : MonoBehaviour
{
    private float moveSpeed; 
    private float dashSpeed;
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

        characterStats.OnStatChanged += UpdateMoveStats;
        UpdateMoveStats();
    }
    private void UpdateMoveStats()
    {
        moveSpeed = characterStats.TotalStats.MoveSpeed;
        dashSpeed = characterStats.TotalStats.DashSpeed;
    }

    public void HandleMove(Vector2 input)
    {
        float targetSpeed = isDashing ? dashSpeed : moveSpeed;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.fixedDeltaTime * acceleration);
        rb.velocity = input * currentSpeed;
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